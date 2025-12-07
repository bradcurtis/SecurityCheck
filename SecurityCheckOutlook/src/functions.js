Office.actions.associate("onMessageSendHandler", async (event) => {
  console.log("[SecurityCheck] Handler invoked");

  try {
    Office.context.mailbox.item.to.getAsync(async (result) => {
      if (result.status === Office.AsyncResultStatus.Succeeded) {
        console.log("[SecurityCheck] Recipients retrieved:", result.value);

        const emails = result.value.map(r => r.emailAddress);
        console.log("[SecurityCheck] Emails extracted:", emails);

        try {
          console.log("[SecurityCheck] Sending request to API...");
          const response = await fetch("https://localhost:5199/api/email/check-multiple", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ emails })
          });

          console.log("[SecurityCheck] API response status:", response.status);
          const results = await response.json();
          console.log("[SecurityCheck] API response body:", results);

          const message = results.map(r =>
            `${r.email}: ${r.secured ? "Secured ✅" : "Unsecured ❌"}`
          ).join("\n");

          console.log("[SecurityCheck] Notification message composed:", message);

          Office.context.mailbox.item.notificationMessages.addAsync("securityCheck", {
            type: "informationalMessage",
            message,
            icon: "icon16",
            persistent: false
          }, (asyncResult) => {
            if (asyncResult.status === Office.AsyncResultStatus.Succeeded) {
              console.log("[SecurityCheck] Notification added successfully");
            } else {
              console.error("[SecurityCheck] Failed to add notification:", asyncResult.error);
            }
          });
        } catch (apiErr) {
          console.error("[SecurityCheck] API call failed:", apiErr);
        }
      } else {
        console.error("[SecurityCheck] Failed to get recipients:", result.error);
      }

      event.completed();
      console.log("[SecurityCheck] Event completed");
    });
  } catch (err) {
    console.error("[SecurityCheck] Unexpected error:", err);
    event.completed();
    console.log("[SecurityCheck] Event completed after error");
  }
});