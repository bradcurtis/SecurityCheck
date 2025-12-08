/* global document, Office */

Office.onReady((info) => {
  if (info.host === Office.HostType.Outlook) {
    document.getElementById("sideload-msg").style.display = "none";
    document.getElementById("app-body").style.display = "flex";
    document.getElementById("run").onclick = run;
  }
});

export async function run() {
  const item = Office.context.mailbox.item;
  let insertAt = document.getElementById("item-subject");

  // Clear previous content
  insertAt.innerHTML = "";

  // Show subject
  let boldLabel = document.createElement("b");
  boldLabel.appendChild(document.createTextNode("Subject: "));
  insertAt.appendChild(boldLabel);

  insertAt.appendChild(document.createElement("br"));
  insertAt.appendChild(document.createTextNode(item.subject));
  insertAt.appendChild(document.createElement("br"));

  // Also call the security check service with recipients
  try {
    Office.context.mailbox.item.to.getAsync(async (result) => {
      if (result.status === Office.AsyncResultStatus.Succeeded) {
        const recipients = result.value.map(r => r.emailAddress);
        console.log("Run button clicked, recipients:", recipients);

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
      } else {
        console.error("Failed to get recipients:", result.error);
      }
    });
  } catch (err) {
    console.error("SecurityCheck API error:", err);
  }
}