using Microsoft.Playwright;
using NUnit.Framework;

namespace SnipeItTests.Base;

public abstract class TestBase
{
    protected IPlaywright Playwright { get; private set; } = null!;
    protected IBrowser Browser { get; private set; } = null!;
    protected IBrowserContext Context { get; private set; } = null!;
    protected IPage Page { get; private set; } = null!;

    [SetUp]
    public async Task Setup()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

        // 1. Check NUnit run parameter first, then environment variable, default to true
        string nunitParam = TestContext.Parameters.Get("Headless", string.Empty);
        string envParam = Environment.GetEnvironmentVariable("HEADLESS") ?? string.Empty;

        bool isHeadless = true; // Default to true

        if (!string.IsNullOrEmpty(nunitParam))
        {
            isHeadless = !nunitParam.Equals("false", StringComparison.OrdinalIgnoreCase);
        }
        else if (!string.IsNullOrEmpty(envParam))
        {
            isHeadless = !envParam.Equals("false", StringComparison.OrdinalIgnoreCase);
        }

        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = isHeadless,
            SlowMo = isHeadless ? 0 : 300
        });

        Context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1280, Height = 800 }
        });

        Page = await Context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        if (Page != null) await Page.CloseAsync();
        if (Context != null) await Context.CloseAsync();
        if (Browser != null) await Browser.CloseAsync();
        Playwright?.Dispose();
    }
}