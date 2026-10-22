using Microsoft.Playwright;

namespace SnipeItTests.Pages
{
    public class AssetsPage
    {
        private readonly IPage _page;

        private ILocator ModelSelect => _page.Locator("#select2-model_id-container");
        private ILocator ModelSearchInput => _page.Locator(".select2-search__field");
        private ILocator StatusSelect => _page.Locator("#select2-status_id-container");
        private ILocator CheckoutToRadio => _page.Locator("input[value='user']");
        private ILocator AssignedUserSelect => _page.Locator("#select2-assigned_to-container");
        private ILocator AssetTagInput => _page.Locator("#tag");
        private ILocator SaveButton => _page.Locator("button[type='submit'].btn.btn-primary");
        private ILocator SearchInput => _page.Locator("input[aria-controls='table']");

        public AssetsPage(IPage page)
        {
            _page = page;
        }

        public async Task NavigateToCreateAssetAsync()
        {
            await _page.GotoAsync("https://demo.snipeitapp.com/hardware/create");
        }

        public async Task<string> CreateMacBookAssetAsync(string assetTag)
        {
            await ModelSelect.ClickAsync();
            await ModelSearchInput.FillAsync("MacBook Pro 13");
            await _page.Locator(".select2-results__option").First.ClickAsync();

            await StatusSelect.ClickAsync();
            await _page.Locator(".select2-results__option", new() { HasText = "Ready to Deploy" }).ClickAsync();

            await CheckoutToRadio.ClickAsync();

            await AssignedUserSelect.ClickAsync();
            var userOptions = _page.Locator(".select2-results__option");
            await userOptions.First.WaitForAsync();
            int count = await userOptions.CountAsync();

            var random = new Random();
            int randomIndex = random.Next(1, count);
            await userOptions.Nth(randomIndex).ClickAsync();

            await AssetTagInput.FillAsync(assetTag);
            await SaveButton.ClickAsync();
            await _page.WaitForURLAsync("**/hardware", new() { WaitUntil = WaitUntilState.DOMContentLoaded });

            return assetTag;
        }

        public async Task SearchAssetAsync(string assetTag)
        {
            await SearchInput.FillAsync(assetTag);
            await _page.Keyboard.PressAsync("Enter");
            await _page.WaitForTimeoutAsync(1500);
        }

        public async Task<bool> IsAssetPresentInListAsync(string assetTag)
        {
            var assetLink = _page.Locator($"table tbody tr td a:text('{assetTag}')");
            return await assetLink.IsVisibleAsync();
        }

        public async Task ClickAssetFromListAsync(string assetTag)
        {
            var assetLink = _page.Locator($"table tbody tr td a:text('{assetTag}')");
            await assetLink.First.ClickAsync();
            await _page.WaitForURLAsync("**/hardware/**", new() { WaitUntil = WaitUntilState.DOMContentLoaded });
        }
    }
}