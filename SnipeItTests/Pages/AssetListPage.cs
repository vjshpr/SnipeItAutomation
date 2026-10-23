using Microsoft.Playwright;
using SnipeItTests.Pages;

namespace SnipeItTests.Pages;

public class AssetsListPage : BasePage
{
    // Locators
    private ILocator SearchInput => Page.Locator("input.search-input, input[type='search']").First;
    private ILocator SuccessAlertBanner => Page.Locator(".alert-success, div.alert-success").First;

    // Header Navigation Dropdown Locators
    private ILocator CreateNewDropdown => Page.Locator("a.dropdown-toggle:has-text('Create New')").First;

    // Target the Asset option cleanly by its href link or text content
    private ILocator AssetDropdownOption => Page.Locator("ul.dropdown-menu a[href*='/hardware/create']").First;

    public AssetsListPage(IPage page) : base(page) { }

    /// <summary>
    /// Navigates to the Asset Creation page by clicking 'Create New' in the top navbar and selecting 'Asset'.
    /// </summary>
    public async Task<AssetCreatePage> ClickCreateNewAsync()
    {
        // 1. Click the top navbar "Create New" dropdown button
        await CreateNewDropdown.ClickAsync();

        // 2. Wait for the dropdown menu to open and click the "Asset" option
        await AssetDropdownOption.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await AssetDropdownOption.ClickAsync();

        // 3. Wait for navigation to the creation form URL (/hardware/create)
        await Page.WaitForURLAsync("**/hardware/create", new()
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        // 4. Wait for the form container to be rendered
        await Page.WaitForSelectorAsync("form#create-form, form");

        return new AssetCreatePage(Page);
    }

    public async Task<string> GetSuccessAlertTextAsync()
    {
        await SuccessAlertBanner.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        return await SuccessAlertBanner.InnerTextAsync();
    }

    public async Task<AssetsListPage> SearchAssetAsync(string assetTag)
    {
        await SearchInput.FillAsync(assetTag);
        await Page.Keyboard.PressAsync("Enter");

        await Page.WaitForResponseAsync(response =>
            response.Url.Contains("/api/v1/hardware") && response.Status == 200);

        return this;
    }

    public async Task<bool> IsAssetInTableAsync(string assetTag)
    {
        var rowCell = Page.GetByRole(AriaRole.Cell, new() { Name = assetTag, Exact = true });
        return await rowCell.IsVisibleAsync();
    }

    public async Task<AssetDetailsPage> OpenAssetDetailsAsync(string assetTag)
    {
        var assetLink = Page.GetByRole(AriaRole.Link, new() { Name = assetTag, Exact = true }).First;
        await assetLink.ClickAsync();

        await Page.WaitForURLAsync(url => url.Contains("/hardware/"), new()
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        return new AssetDetailsPage(Page);
    }
}