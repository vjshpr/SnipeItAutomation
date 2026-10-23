using NUnit.Framework;
using SnipeItTests.Base;
using SnipeItTests.Models;
using SnipeItTests.Pages;

namespace SnipeItTests.Tests;

[TestFixture]
[Explicit("Run manually when isolated module testing is required")]
public class AssetDetailValidationTests : TestBase
{
    [Test]
    [Description("Navigates from list grid to created asset detail page and validates header, tag, serial number, model, and status.")]
    public async Task Verify_Created_Asset_Details_On_Detail_Page()
    {
        // Arrange
        var testAsset = new AssetModel();
        var loginPage = new LoginPage(Page);

        // 1. Login
        var assetsListPage = await (await loginPage.NavigateAsync()).LoginAsAdminAsync();

        // 2. Create Asset via 'Create New' -> 'Asset'
        var createPage = await assetsListPage.ClickCreateNewAsync();
        var (listPage, capturedTag) = await createPage.CreateAndCheckoutAssetAsync(testAsset);
        assetsListPage = listPage;

        // 3. Search for the asset in the list table
        await assetsListPage.SearchAssetAsync(capturedTag);

        // 4. Assert Asset exists in grid list
        bool isAssetVisible = await assetsListPage.IsAssetInTableAsync(capturedTag);
        Assert.That(isAssetVisible, Is.True, $"Asset tag '{capturedTag}' was not found in assets list table.");

        // 5. Navigate to the Asset Detail page
        var detailsPage = await assetsListPage.OpenAssetDetailsAsync(capturedTag);

        // 6. Read detail page header and details container text
        string headerTitle = await detailsPage.GetAssetHeaderTitleAsync();
        string detailsSummary = await detailsPage.GetAssetDetailsSummaryTextAsync();

        // 7. Validate all asset creation fields on the detail page
        Assert.Multiple(() =>
        {
            Assert.That(headerTitle, Does.Contain(capturedTag),
                $"Page header title '{headerTitle}' does not contain captured tag '{capturedTag}'.");

            Assert.That(detailsSummary, Does.Contain(testAsset.ModelName).IgnoreCase,
                $"Details summary does not contain expected model '{testAsset.ModelName}'. Summary text: '{detailsSummary}'");

            Assert.That(detailsSummary, Does.Contain(testAsset.SerialNumber).IgnoreCase,
                $"Details summary does not contain expected serial number '{testAsset.SerialNumber}'. Summary text: '{detailsSummary}'");

            Assert.That(detailsSummary, Does.Contain(testAsset.Status).IgnoreCase,
                $"Details summary does not contain expected status '{testAsset.Status}'. Summary text: '{detailsSummary}'");
        });
    }
}