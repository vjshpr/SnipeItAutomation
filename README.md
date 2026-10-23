# Snipe-IT End-to-End Automation Framework (.NET 8 & Playwright)

A production-ready test automation framework targeting the [Snipe-IT Asset Management Demo](https://demo.snipeitapp.com), designed using C#, .NET 8, Playwright, and NUnit.

## Key Architectural Features

- **Page Object Model (POM):** Clean Separation of Concerns with a Fluent API design.
- **User-Centric Locators:** Priority given to Accessibility Role (`GetByRole`) and text selectors over fragile DOM structure.
- **Network Synchronization:** Dynamic wait strategies listening to AJAX network responses (`WaitForResponseAsync`) rather than static sleeps.
- **Diagnostic Tracing:** Automatic capture of Playwright Tracing (`.zip` containing snapshots, action logs, and network calls) on test failure.
- **Parallelization Ready:** NUnit parallel execution support configured out of the box.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- PowerShell or Terminal

## Quick Start

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/YOUR_USERNAME/SnipeItAutomation.git](https://github.com/YOUR_USERNAME/SnipeItAutomation.git)
   cd SnipeItAutomation
