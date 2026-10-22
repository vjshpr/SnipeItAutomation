using SnipeItTests.Base;
using SnipeItTests.Pages;

namespace SnipeItTests.Tests
{
    [TestFixture]
    public class AssetWorkflowTests : TestBase
    {
        [Test]
        public async Task VerifyMacBookAssetCreationAndCheckoutFlow()
        {
            var loginPage = new LoginPage(Page);
            var assetsPage = new AssetsPage(Page);
            var assetDetailsPage = new AssetDetailsPage(Page);

            string assetTag = $"MBP-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";

            await loginPage.GotoAsync("https://demo.snipeitapp.com/login");
            await loginPage.LoginAsync("admin", "password");

            await assetsPage.NavigateToCreateAssetAsync();
            await assetsPage.CreateMacBookAssetAsync(assetTag);

            await assetsPage.SearchAssetAsync(assetTag);
            bool isPresent = await assetsPage.IsAssetPresentInListAsync(assetTag);
            Assert.That(isPresent, Is.True, $"Asset with tag '{assetTag}' was not found in list.");

            await assetsPage.ClickAssetFromListAsync(assetTag);
            string headerTitle = await assetDetailsPage.GetPageHeaderTitleAsync();
            Assert.That(headerTitle, Does.Contain(assetTag), "Asset header does not match asset tag.");

            await assetDetailsPage.NavigateToHistoryTabAsync();
            bool hasDeploymentHistory = await assetDetailsPage.VerifyHistoryContainsActionAsync("Deploy");
            Assert.That(hasDeploymentHistory, Is.True, "Deployment record not found in History tab.");
        }
    }
}