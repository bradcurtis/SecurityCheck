# SecurityCheck

SecurityCheck is an Outlook add‑in sample that validates email recipients (To, CC, BCC) against a security service before sending. It demonstrates how to integrate a backend API with an Office add‑in manifest and ribbon commands.

---

## 📂 Repository Structure
- **SecurityCheckAPI/** – Core API project for recipient validation.
- **EmailSecurityApi.Tests/** – Unit tests for deterministic validation outcomes.
- **Manifest/** – Example Outlook add‑in manifest files.
- **SecurityCheckSolution.sln** – Visual Studio solution.

---

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2022 or later with .NET 6 SDK.
- Outlook Web (https://outlook.office.com) or Microsoft 365 desktop Outlook.
- Node.js if using Office Add‑in CLI for sideloading.

### Clone the repo
```bash
git clone https://github.com/bradcurtis/SecurityCheck.git
cd SecurityCheck