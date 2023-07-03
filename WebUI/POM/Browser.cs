namespace NordPassHomeWorkTAF.WebUI.POM
{
    public class Browser
    {
        public IBrowser PlaywrightBrowser { get; private set; }
        public IBrowserContext PlaywrightBrowserContext { get; set; }
        public IPage Page { get; private set; }

        public Browser()
        {
        }

        public async Task InitializeAsync()
        {
            var playwright = await Playwright.CreateAsync();
            PlaywrightBrowser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                Args = new[] { "--start-maximized" }
            });
            PlaywrightBrowserContext = await PlaywrightBrowser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = ViewportSize.NoViewport
            });
            Page = await PlaywrightBrowserContext.NewPageAsync();
        }


        public async Task GoToUrlAsync(string url)
        {
            await Page.GotoAsync(url);
        }

        public async Task WaitForSelectorAsync(string selector)
        {
            await Page.WaitForSelectorAsync(selector);
        }

        public async Task ClickAsync(string selector)
        {
            await Page.ClickAsync(selector);
        }

        public string GetUrl()
        {
            return Page.Url;
        }
    }
}
