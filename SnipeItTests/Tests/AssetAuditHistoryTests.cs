using NUnit.Framework;
using SnipeItTests.Base;
using SnipeItTests.Models;
using SnipeItTests.Pages;

namespace SnipeItTests.Tests;

[TestFixture]
[Explicit("Run manually when isolated module testing is required")]
public class AssetAuditHistoryTests : TestBase
{
    [Test]
    [Description("Verifies that creating and checking out an asset generates an audit log record under the History tab.")]
    public async Task Verify_Asset_Audit_History_Log()
    {
        // 1. Arrange & Login
        var testAsset = new AssetModel();
        var loginPage = new LoginPage(Page);
        var assetsListPage = await (await loginPage.NavigateAsync()).LoginAsAdminAsync();

        // 2. Create Asset & Capture Tag
        var createPage = await assetsListPage.ClickCreateNewAsync();
        var (listPage, capturedTag) = await createPage.CreateAndCheckoutAssetAsync(testAsset);

        // 3. Search & Navigate to Detail Page
        await listPage.SearchAssetAsync(capturedTag);
        var detailsPage = await listPage.OpenAssetDetailsAsync(capturedTag);

        // 4. Click History Tab
        await detailsPage.ClickHistoryTabAsync();

        // 5. Assert Audit Log Row Content
        string latestRecord = await detailsPage.GetLatestHistoryRecordTextAsync();

        Assert.Multiple(() =>
        {
            Assert.That(latestRecord, Does.Contain("create new").Or.Contain("checkout").Or.Contain("created").IgnoreCase,
                $"Expected audit trail to contain creation event, but got: '{latestRecord}'");
        });
    }
}