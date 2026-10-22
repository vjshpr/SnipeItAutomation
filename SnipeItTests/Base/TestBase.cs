using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace SnipeItTests.Base
{
    public class TestBase : PageTest
    {
        protected override TimeSpan Timeout => TimeSpan.FromSeconds(30);

        [SetUp]
        public async Task SetupBase()
        {
            await Context.Tracing.StartAsync(new()
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        [TearDown]
        public async Task TearDownBase()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var failedTestName = TestContext.CurrentContext.Test.Name;
                await Context.Tracing.StopAsync(new()
                {
                    Path = $"trace-{failedTestName}-{DateTime.Now:yyyyMMdd_HHmmss}.zip"
                });
            }
        }
    }
}