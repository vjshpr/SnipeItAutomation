using Microsoft.Playwright;

namespace SnipeItTests.Pages;

public abstract class BasePage
{
    protected readonly IPage Page;

    protected BasePage(IPage page)
    {
        Page = page;
    }

    public async Task SelectOptionFromSelect2Async(ILocator dropdownTrigger, string optionText)
    {
        await dropdownTrigger.ClickAsync();

        var searchInput = Page.Locator(".select2-container--open .select2-search__field").First;
        if (await searchInput.IsVisibleAsync())
        {
            await searchInput.FillAsync(optionText);
            await Page.WaitForTimeoutAsync(500); // Allow AJAX debounce
        }

        var option = Page.Locator(".select2-results__option")
            .Filter(new() { HasText = optionText })
            .First;

        await option.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await option.ClickAsync();
    }

    public async Task SelectRandomOptionFromSelect2Async(ILocator dropdownTrigger)
    {
        await dropdownTrigger.ClickAsync();

        // Wait for Select2 results container to be visible
        var resultsContainer = Page.Locator(".select2-results__options, ul.select2-results__options").First;
        await resultsContainer.WaitForAsync(new() { State = WaitForSelectorState.Visible });

        // Filter out loading indicators or disabled items
        var options = Page.Locator(".select2-results__option:not(.select2-results__option--load-more):not([aria-disabled='true'])");

        // Wait up to 5s for options to load from API call
        try
        {
            await options.First.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 5000 });
        }
        catch (TimeoutException)
        {
            // If AJAX needs search input, type a space to trigger search results
            var searchInput = Page.Locator(".select2-container--open .select2-search__field").First;
            if (await searchInput.IsVisibleAsync())
            {
                await searchInput.FillAsync(" ");
                await options.First.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            }
        }

        int count = await options.CountAsync();
        if (count == 0)
        {
            throw new InvalidOperationException("No selectable options found in dropdown.");
        }

        // Pick the first valid option
        await options.First.ClickAsync();
    }
}