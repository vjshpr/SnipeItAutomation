using NUnit.Framework;
using SnipeItTests.Base;
using SnipeItTests.Models;
using SnipeItTests.Pages;

namespace SnipeItTests.Tests;

[TestFixture]
public class AssetWorkflowTests : TestBase
{
    [Test]
    [Description("Full End-to-End Suite: Login -> Create Asset & Checkout -> Verify Success Banner -> Search Grid -> Detail Page Validation -> Audit History Log Validation.")]
    public async Task Verify_Full_Asset_Lifecycle_Workflow()
    {
        // 1. Arrange & Model Setup
        var testAsset = new AssetModel();
        var loginPage = new LoginPage(Page);

        // --- Module 1: Authentication ---
        var assetsListPage = await (await loginPage.NavigateAsync()).LoginAsAdminAsync();
        TestContext.Out.WriteLine("[SUCCESS] Module 1: Authentication - Passed");

        // --- Module 2: Navigation & Form Submission ---
        var createPage = await assetsListPage.ClickCreateNewAsync();
        var (listPage, capturedTag) = await createPage.CreateAndCheckoutAssetAsync(testAsset);
        assetsListPage = listPage;
        TestContext.Out.WriteLine("[SUCCESS] Module 2: Asset Creation & Checkout Form - Passed");

        // --- Module 3: Feedback & Verification ---
        string successMessageText = await assetsListPage.GetSuccessAlertTextAsync();
        Assert.Multiple(() =>
        {
            Assert.That(successMessageText, Does.Contain("Success").IgnoreCase, "Success banner was not displayed.");
            Assert.That(successMessageText, Does.Contain(capturedTag), $"Success banner did not contain asset tag '{capturedTag}'.");
        });
        TestContext.Out.WriteLine("[SUCCESS] Module 3: Success Banner Verification - Passed");

        // --- Module 4: Grid Search & Detail Page Field Validation ---
        await assetsListPage.SearchAssetAsync(capturedTag);
        bool isAssetVisible = await assetsListPage.IsAssetInTableAsync(capturedTag);
        Assert.That(isAssetVisible, Is.True, $"Asset tag '{capturedTag}' was not found in the grid list.");

        var detailsPage = await assetsListPage.OpenAssetDetailsAsync(capturedTag);

        string headerTitle = await detailsPage.GetAssetHeaderTitleAsync();
        string detailsSummary = await detailsPage.GetAssetDetailsSummaryTextAsync();

        Assert.Multiple(() =>
        {
            Assert.That(headerTitle, Does.Contain(capturedTag), $"Header title '{headerTitle}' does not match tag '{capturedTag}'.");
            Assert.That(detailsSummary, Does.Contain(testAsset.ModelName).IgnoreCase, $"Details summary does not contain model '{testAsset.ModelName}'.");
            Assert.That(detailsSummary, Does.Contain(testAsset.SerialNumber).IgnoreCase, $"Details summary does not contain serial '{testAsset.SerialNumber}'.");
            Assert.That(detailsSummary, Does.Contain(testAsset.Status).IgnoreCase, $"Details summary does not contain status '{testAsset.Status}'.");
        });
        TestContext.Out.WriteLine("[SUCCESS] Module 4: Grid Search & Asset Details Field Validation - Passed");

        // --- Module 5: Audit Trail & History Verification ---
        await detailsPage.ClickHistoryTabAsync();
        string latestHistoryRecord = await detailsPage.GetLatestHistoryRecordTextAsync();

        Assert.That(latestHistoryRecord, Does.Contain("create new").Or.Contain("checkout").Or.Contain("created").IgnoreCase,
            $"Expected audit trail to contain creation event, but got: '{latestHistoryRecord}'");
        TestContext.Out.WriteLine("[SUCCESS] Module 5: Audit History Log Verification - Passed");

        // Final Summary Report Output
        TestContext.Out.WriteLine("=================================================");
        TestContext.Out.WriteLine("🎉 ALL 5 MODULES PASSED SUCCESSFULLY IN ONE GO!");
        TestContext.Out.WriteLine("1. Module 1: Authentication [PASSED]");
        TestContext.Out.WriteLine("2. Module 2: Navigation & Form Submission [PASSED]");
        TestContext.Out.WriteLine("3. Module 3: Success Banner Verification [PASSED]");
        TestContext.Out.WriteLine("4. Module 4: Grid & Detail Page Validation [PASSED]");
        TestContext.Out.WriteLine("5. Module 5: Audit History Log Verification [PASSED]");
        TestContext.Out.WriteLine("=================================================");
    }
}