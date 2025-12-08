/* eslint-disable no-undef */

// Main implementation
function verifySecurity(event) {
  try {
    Office.context.mailbox.item.to.getAsync(async (result) => {
      if (result.status === Office.AsyncResultStatus.Succeeded) {
        const recipients = result.value.map(r => r.emailAddress);

        console.log("verifySecurity button clicked, recipients:", recipients);

        const response = await fetch("https://localhost:7128/api/email/check-multiple", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ emails: recipients })
        });

        const data = await response.json();

        let message = data.results
          .map(r => `${r.email}: ${r.secured ? "Secured ✅" : "Not Secured ❌"}`)
          .join("\n");

        Office.context.mailbox.item.notificationMessages.replaceAsync("securityCheck", {
          type: Office.MailboxEnums.ItemNotificationMessageType.InformationalMessage,
          message: message,
          icon: "Icon.16x16",
          persistent: false
        });

        event.completed(); // safe to call here after notification
      } else {
        console.error("Failed to get recipients:", result.error);
        event.completed();
      }
    });
  } catch (err) {
    console.error("SecurityCheck API error:", err);
    event.completed();
  }
}

// Guarded association inside Office.onReady
Office.onReady(() => {
  if (typeof verifySecurity === "function") {
    if (!Office._verifySecurityAssociated) {
      Office.actions.associate("verifySecurity", verifySecurity);
      Office._verifySecurityAssociated = true; // flag to prevent duplicates
      console.log("verifySecurity associated successfully");
    } else {
      console.warn("verifySecurity already associated, skipping duplicate registration");
    }
  } else {
    console.error("verifySecurity is not defined or not a function");
  }
});