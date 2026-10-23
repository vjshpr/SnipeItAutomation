using NUnit.Framework;
using SnipeItTests.Base;
using SnipeItTests.Models;
using SnipeItTests.Pages;

namespace SnipeItTests.Tests;

[TestFixture]
[Explicit("Run manually when isolated module testing is required")]
public class AssetVerificationTests : TestBase
{
    [Test]
    [Description("Tests finding the created asset in the assets list grid and navigating to its details page.")]
    public async Task Verify_Asset_List_Search_And_Detail_Navigation()
    {
        // Arrange
        var testAsset = new AssetModel();
        var loginPage = new LoginPage(Page);

        // 1. Login and Create Asset
        var assetsListPage = await (await loginPage.NavigateAsync()).LoginAsAdminAsync();
        var createPage = await assetsListPage.ClickCreateNewAsync();
        var (listPage, capturedTag) = await createPage.CreateAndCheckoutAssetAsync(testAsset);
        assetsListPage = listPage;

        // 2. Search for the created asset tag
        await assetsListPage.SearchAssetAsync(capturedTag);

        // 3. Assert asset is present in grid
        bool isAssetVisible = await assetsListPage.IsAssetInTableAsync(capturedTag);
        Assert.That(isAssetVisible, Is.True, $"Asset '{capturedTag}' was not found in search results.");

        // 4. Navigate to Asset Details page and validate header
        var detailsPage = await assetsListPage.OpenAssetDetailsAsync(capturedTag);
        string headerTitle = await detailsPage.GetAssetHeaderTitleAsync();
        Assert.That(headerTitle, Does.Contain(capturedTag), "Asset detail header does not match captured asset tag.");
    }
}