using NUnit.Framework;
using SnipeItTests.Base;
using SnipeItTests.Models;
using SnipeItTests.Pages;

namespace SnipeItTests.Tests;

[TestFixture]
[Explicit("Run manually when isolated module testing is required")]
public class AssetCreationTests : TestBase
{
    [Test]
    [Description("Tests creating a new asset and verifying the success message banner.")]
    public async Task Verify_Asset_Creation_Success_Message()
    {
        // Arrange
        var testAsset = new AssetModel();
        var loginPage = new LoginPage(Page);

        // 1. Log in
        var assetsListPage = await (await loginPage.NavigateAsync()).LoginAsAdminAsync();

        // 2. Open Create Asset page and submit form
        var createPage = await assetsListPage.ClickCreateNewAsync();
        var (listPage, capturedTag) = await createPage.CreateAndCheckoutAssetAsync(testAsset);
        assetsListPage = listPage;

        // 3. Verify Success Banner Message
        string successMessage = await assetsListPage.GetSuccessAlertTextAsync();

        Assert.Multiple(() =>
        {
            Assert.That(successMessage, Does.Contain("Success"), "Success banner text was not displayed.");
            Assert.That(successMessage, Does.Contain(capturedTag), $"Success banner did not contain expected asset tag '{capturedTag}'.");
        });
    }
}