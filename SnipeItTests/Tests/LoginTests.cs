using Microsoft.Playwright;
using NUnit.Framework;
using SnipeItTests.Base;
using SnipeItTests.Pages;

namespace SnipeItTests.Tests;

[TestFixture]
[Explicit("Run manually when isolated module testing is required")]
public class LoginTests : TestBase
{
    [Test]
    [Description("Tests only the login functionality and verifies successful navigation to the dashboard.")]
    public async Task Verify_Login_Functionality()
    {
        // Arrange
        var loginPage = new LoginPage(Page);

        // Act
        var assetsListPage = await (await loginPage.NavigateAsync()).LoginAsAdminAsync();

        // Assert
        // Verify we landed on the dashboard page
        Assert.That(Page.Url, Does.Not.Contain("/login"), "User was not logged in successfully.");
    }
}