using Microsoft.Playwright;
using SnipeItTests.Pages;

namespace SnipeItTests.Pages;

public class AssetDetailsPage : BasePage
{
    // Locators
    private ILocator PageHeader => Page.Locator("h1.pagetitle, h1").First;
    private ILocator DetailBox => Page.Locator(".content-wrapper section.content, div.box-body").First;

    // History Tab Locators
    private ILocator HistoryTab => Page.Locator("a[href='#history'], .nav-tabs a:has-text('History')").First;

    // Target populated history data rows (excluding the temporary 'no-records-found' placeholder row)
    private ILocator HistoryDataRows => Page.Locator("#history-table tbody tr:not(.no-records-found), #history tbody tr:not(.no-records-found)");

    public AssetDetailsPage(IPage page) : base(page) { }

    public async Task<string> GetAssetHeaderTitleAsync()
    {
        await PageHeader.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        return await PageHeader.InnerTextAsync();
    }

    public async Task<string> GetAssetDetailsSummaryTextAsync()
    {
        await DetailBox.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        return await DetailBox.InnerTextAsync();
    }

    /// <summary>
    /// Navigates to the History tab on the Asset Details view and waits for AJAX history data to load.
    /// </summary>
    public async Task ClickHistoryTabAsync()
    {
        await HistoryTab.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await HistoryTab.ClickAsync();

        // Wait specifically for populated AJAX audit records to render
        await HistoryDataRows.First.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 15000 });
    }

    /// <summary>
    /// Gets the inner text of the most recent audit entry (top populated row) in the History table.
    /// </summary>
    public async Task<string> GetLatestHistoryRecordTextAsync()
    {
        return await HistoryDataRows.First.InnerTextAsync();
    }
}