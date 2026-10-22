using Microsoft.Playwright;

namespace SnipeItTests.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        private ILocator UsernameInput => _page.Locator("#username");
        private ILocator PasswordInput => _page.Locator("#password");
        private ILocator LoginButton => _page.Locator("button[type='submit']");

        public LoginPage(IPage page)
        {
            _page = page;
        }

        public async Task GotoAsync(string url)
        {
            await _page.GotoAsync(url);
        }

        public async Task LoginAsync(string username, string password)
        {
            await UsernameInput.FillAsync(username);
            await PasswordInput.FillAsync(password);
            await LoginButton.ClickAsync();
            await _page.WaitForURLAsync("**/hardware**", new() { WaitUntil = WaitUntilState.DOMContentLoaded });
        }
    }
}