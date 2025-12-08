/* eslint-disable no-undef */

// Manual button implementation (still useful for ribbon commands)
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

// OnSend handler — automatically runs when user clicks Send
function validateRecipientsOnSend(event) {
  // Helper to collect emails from a recipient field
  function getEmails(field) {
    return new Promise(resolve => {
      Office.context.mailbox.item[field].getAsync(result => {
        if (result.status === Office.AsyncResultStatus.Succeeded) {
          resolve(result.value.map(r => r.emailAddress));
        } else {
          resolve([]);
        }
      });
    });
  }

  (async () => {
    try {
      const toRecipients = await getEmails("to");
      const ccRecipients = await getEmails("cc");
      const bccRecipients = await getEmails("bcc");

      const allRecipients = [...toRecipients, ...ccRecipients, ...bccRecipients];
      console.log("OnSend recipients:", allRecipients);

      const response = await fetch("https://localhost:7128/api/email/check-multiple", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ emails: allRecipients })
      });

      const data = await response.json();
      const unsecured = data.results.filter(r => !r.secured);

      if (unsecured.length > 0) {
        Office.context.mailbox.item.notificationMessages.addAsync("securityWarning", {
          type: Office.MailboxEnums.ItemNotificationMessageType.ErrorMessage,
          message: "Unsecured recipients: " + unsecured.map(u => u.email).join(", ")
        });
        event.completed({ allowEvent: false }); // cancel send
      } else {
        event.completed({ allowEvent: true }); // allow send
      }
    } catch (err) {
      console.error("OnSend API error:", err);
      // Fail safe: allow send if API fails
      event.completed({ allowEvent: true });
    }
  })();
}

// NEW: Ribbon button to validate To, CC, and BCC together
function validateAllRecipients(event) {
  function getEmails(field) {
    return new Promise(resolve => {
      Office.context.mailbox.item[field].getAsync(result => {
        if (result.status === Office.AsyncResultStatus.Succeeded) {
          resolve(result.value.map(r => r.emailAddress));
        } else {
          console.error(`Failed to get ${field}:`, result.error);
          resolve([]);
        }
      });
    });
  }

  (async () => {
    try {
      const toRecipients = await getEmails("to");
      const ccRecipients = await getEmails("cc");
      const bccRecipients = await getEmails("bcc");

      const allRecipients = [...toRecipients, ...ccRecipients, ...bccRecipients];
      console.log("validateAllRecipients button clicked, recipients:", allRecipients);

      const response = await fetch("https://localhost:7128/api/email/check-multiple", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ emails: allRecipients })
      });

      const data = await response.json();
      const message = data.results
        .map(r => `${r.email}: ${r.secured ? "Secured ✅" : "Not Secured ❌"}`)
        .join("\n");

      Office.context.mailbox.item.notificationMessages.replaceAsync("securityCheckAll", {
        type: Office.MailboxEnums.ItemNotificationMessageType.InformationalMessage,
        message: message,
        icon: "Icon.16x16",
        persistent: false
      });

      event.completed();
    } catch (err) {
      console.error("Validation API error:", err);
      event.completed();
    }
  })();
}

// Guarded association inside Office.onReady
Office.onReady(() => {
  if (typeof verifySecurity === "function") {
    if (!Office._verifySecurityAssociated) {
      Office.actions.associate("verifySecurity", verifySecurity);
      Office._verifySecurityAssociated = true;
      console.log("verifySecurity associated successfully");
    }
  }

  /*
    This is non functional for the current environments due to limitations with
    the OnSend feature in Outlook add-ins. Keep this here for when the feature
    is more widely supported.
  */
  if (typeof validateRecipientsOnSend === "function") {
    if (!Office._validateRecipientsAssociated) {
      Office.actions.associate("validateRecipientsOnSend", validateRecipientsOnSend);
      Office._validateRecipientsAssociated = true;
      console.log("validateRecipientsOnSend associated successfully");
    }
  }

  // Associate the new all-recipients validation button
  if (typeof validateAllRecipients === "function") {
    if (!Office._validateAllRecipientsAssociated) {
      Office.actions.associate("validateAllRecipients", validateAllRecipients);
      Office._validateAllRecipientsAssociated = true;
      console.log("validateAllRecipients associated successfully");
    }
  }
});