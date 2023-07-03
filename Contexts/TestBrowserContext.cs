using Microsoft.Extensions.Logging;
using NordPassHomeWorkTAF.WebUI.POM;

namespace NordPassHomeWorkTAF.Contexts
{
    public class TestBrowserContext
    {
        public Browser Browser { get; set; }
        public IBrowserContext PlaywrightBrowserContext { get; set; }
        public Dictionary<string, Uri> Endpoints { get; set; }
        public IReadOnlyList<IPage> Pages => PlaywrightBrowserContext.Pages;

        private readonly ILogger _logger;

        public TestBrowserContext(ILogger logger)
        {
            _logger = logger;
        }

        public async Task RefreshPage()
        {
            await Browser.Page.ReloadAsync();
        }
    }
}
