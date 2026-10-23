using Microsoft.Playwright;
using SnipeItTests.Models;
using SnipeItTests.Pages;

namespace SnipeItTests.Pages;

public class AssetCreatePage : BasePage
{
    // Form Locators
    private ILocator CompanyDropdown => Page.Locator("[id^='select2-company_']").First;
    private ILocator AssetTagInput => Page.Locator("input#asset_tag, input[name='asset_tag']").First;
    private ILocator SerialInput => Page.Locator("input#serials, input#serial_number, input[name*='serial']").First;
    private ILocator ModelDropdown => Page.Locator("[id^='select2-model_']").First;
    private ILocator StatusDropdown => Page.Locator("[id^='select2-status_']").First;

    // Checkout To -> Asset Radio Option (Targets label/input wrapper)
    private ILocator CheckoutAssetRadio => Page.Locator("input[value='asset']");
    private ILocator CheckoutAssetLabel => Page.Locator("label:has(input[value='asset']), .radio label:has-text('Asset')").First;
    private ILocator AssignedAssetDropdown => Page.Locator("#assigned_asset_select .select2-selection, [id^='select2-assigned_asset_']").First;

    // Actions & Banners
    private ILocator SaveButton => Page.Locator("form#create-form button[type='submit'], .box-footer button[type='submit']").First;
    private ILocator SuccessAlertBanner => Page.Locator(".alert-success, div.alert-success").First;

    public AssetCreatePage(IPage page) : base(page) { }

    public async Task<(AssetsListPage ListPage, string CapturedTag)> CreateAndCheckoutAssetAsync(AssetModel asset)
    {
        // 1. Select Company
        await SelectRandomOptionFromSelect2Async(CompanyDropdown);

        // 2. Capture Pre-populated Asset Tag value
        await AssetTagInput.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        string capturedTag = await AssetTagInput.InputValueAsync();

        // 3. Enter Serial Number
        if (await SerialInput.IsVisibleAsync())
        {
            await SerialInput.FillAsync(asset.SerialNumber);
        }

        // 4. Select Model
        await SelectOptionFromSelect2Async(ModelDropdown, asset.ModelName);

        // 5. Select Status
        await SelectOptionFromSelect2Async(StatusDropdown, asset.Status);

        // 6. Under CheckOut To, click the visible Label container or trigger change event directly
        if (await CheckoutAssetLabel.IsVisibleAsync())
        {
            await CheckoutAssetLabel.ClickAsync();
        }
        else
        {
            // Fallback: Programmatically check the hidden radio & dispatch change event
            await CheckoutAssetRadio.EvaluateAsync("el => { el.checked = true; el.dispatchEvent(new Event('change', { bubbles: true })); }");
        }

        // 7. Select the first available value from Target Asset dropdown
        await SelectRandomOptionFromSelect2Async(AssignedAssetDropdown);

        // 8. Click Save button at the bottom
        await SaveButton.ClickAsync();

        // 9. Wait for navigation back to hardware list page
        await Page.WaitForURLAsync(url => url.Contains("/hardware"), new()
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        // 10. Wait explicitly for the green success banner
        await SuccessAlertBanner.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 15000
        });

        return (new AssetsListPage(Page), capturedTag);
    }
}