function checkRecipientsSecurity(event) {
  try {
    Office.context.mailbox.item.to.getAsync(async (result) => {
      if (result.status === Office.AsyncResultStatus.Succeeded) {
        const recipients = result.value.map(r => r.emailAddress);

        console.log("CheckRecipientsSecurity button clicked, recipients:", recipients);

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

Office.actions.associate("checkRecipientsSecurity", checkRecipientsSecurity);