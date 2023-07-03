namespace NordPassHomeWorkTAF.WebUI.POM.Elements
{
    public class CredibilityHomePageSection : BasePageElement
    {
        private ILocator Locator { get; }
        public CredibilityHomePageSection(IPage page) : base(page, "//div[@id='Credibility - homepage']")
        {
            Locator = GetLocator();
        }

        public async Task<bool> IsVisibleAsync()
        {
            return await Locator.IsVisibleAsync();
        }

        public async Task<string> GetBusinessClientsAsync()
        {
            var businessClientsLocator = Locator.Locator("//p[text()='Business clients']");
            return await businessClientsLocator.InnerTextAsync();
        }

        public async Task<string> GetMediaPresenceAsync()
        {
            var mediaPresenceLocator = Locator.Locator("//p[text()='Media presence around the world']");
            return await mediaPresenceLocator.InnerTextAsync();
        }

        public async Task<string> GetTrustpilotRatingAsync()
        {
            var trustpilotRatingLocator = Locator.Locator("//p[text()='Trustpilot Rating']");
            return await trustpilotRatingLocator.InnerTextAsync();
        }

        public async Task<string> GetUsersWorldwideAsync()
        {
            var usersWorldwideLocator = Locator.Locator("//p[text()='Users worldwide']");
            return await usersWorldwideLocator.InnerTextAsync();
        }
    }

}
