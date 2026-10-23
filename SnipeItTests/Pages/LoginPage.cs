using Microsoft.Playwright;
using SnipeItTests.Configuration;

namespace SnipeItTests.Pages;

public class LoginPage : BasePage
{
    private ILocator UsernameInput => Page.GetByPlaceholder("Username");
    private ILocator PasswordInput => Page.GetByPlaceholder("Password");
    private ILocator SubmitButton => Page.GetByRole(AriaRole.Button, new() { Name = "Login" });

    public LoginPage(IPage page) : base(page) { }

    public async Task<LoginPage> NavigateAsync()
    {
        await Page.GotoAsync($"{TestConfig.BaseUrl}/login");
        return this;
    }

    public async Task<AssetsListPage> LoginAsAdminAsync()
    {
        await UsernameInput.FillAsync(TestConfig.AdminUsername);
        await PasswordInput.FillAsync(TestConfig.AdminPassword);
        await SubmitButton.ClickAsync();

        await Page.WaitForURLAsync(url => url.Contains("snipeitapp") || url.EndsWith(".com/"), new()
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        return new AssetsListPage(Page);
    }
}