Snipe-IT Automation Suite

A Playwright C# end-to-end test automation framework for Snipe-IT Asset Management.

🚀 Quick Start

1. Run Headless (Default)

Run the test suite silently in the background:

dotnet test --logger "console;verbosity=detailed"


2. Run Headed (Visible Browser)

Run tests with a visible browser window:

dotnet test -- TestRunParameters.Parameter(name="Headless",value="false")


🧪 Modules Tested

Module 1 (Authentication): Logs in as Administrator.

Module 2 (Asset Creation): Navigates to Create Asset, fills details, and checks out to an asset.

Module 3 (Feedback): Captures dynamic Asset Tag and verifies the green success banner.

Module 4 (Detail Validation): Searches grid, opens asset detail page, and validates field values.

Module 5 (Audit History): Navigates to History tab and verifies the create new audit log record.

📁 Tech Stack

Language: C# / .NET 10

Framework: Playwright C# & NUnit

Pattern: Page Object Model (POM)
