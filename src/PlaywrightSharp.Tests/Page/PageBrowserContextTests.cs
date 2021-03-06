using PlaywrightSharp.Tests.BaseTests;
using Xunit;
using Xunit.Abstractions;

namespace PlaywrightSharp.Tests.Page
{
    ///<playwright-file>page.spec.js</playwright-file>
    ///<playwright-describe>Page.browserContext</playwright-describe>
    public class PageBrowserContextTests : PlaywrightSharpPageBaseTest
    {
        internal PageBrowserContextTests(ITestOutputHelper output) : base(output)
        {
        }

        ///<playwright-file>page.spec.js</playwright-file>
        ///<playwright-describe>Page.browserContext</playwright-describe>
        ///<playwright-it>should return the correct browser instance</playwright-it>
        [Fact]
        public void ShouldReturnTheCorrectBrowserInstance() => Assert.Equal(Context, Page.BrowserContext);
    }
}
