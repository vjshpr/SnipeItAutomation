using Microsoft.Playwright;

namespace SnipeItTests.Pages
{
    public class AssetDetailsPage
    {
        private readonly IPage _page;

        private ILocator AssetTitleHeading => _page.Locator("section.content h1");
        private ILocator HistoryTab => _page.Locator("a[href='#history']");
        private ILocator HistoryTableRows => _page.Locator("#history table tbody tr");

        public AssetDetailsPage(IPage page)
        {
            _page = page;
        }

        public async Task<string> GetPageHeaderTitleAsync()
        {
            return await AssetTitleHeading.InnerTextAsync();
        }

        public async Task NavigateToHistoryTabAsync()
        {
            await HistoryTab.ClickAsync();
            await _page.WaitForSelectorAsync("#history.active");
        }

        public async Task<bool> VerifyHistoryContainsActionAsync(string actionKeyword)
        {
            var historyText = await HistoryTableRows.First.InnerTextAsync();
            return historyText.Contains(actionKeyword, StringComparison.OrdinalIgnoreCase);
        }
    }
}