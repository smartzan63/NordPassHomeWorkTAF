namespace NordPassHomeWorkTAF.WebUI.POM.Elements
{
    public class HeroHomePageSection : BasePageElement
    {
        private ILocator Locator { get; }
        public HeroHomePageSection(IPage page) : base(page, "//div[@id='Hero - homepage']")
        {
            Locator = GetLocator();
        }

        public async Task<bool> IsVisibleAsync()
        {
            return await Locator.IsVisibleAsync();
        }

        public async Task<string> GetTitleAsync()
        {
            var titleLocator = Locator.Locator("h1");
            return await titleLocator.InnerTextAsync();
        }

        public async Task<string> GetDescriptionAsync()
        {
            var descriptionLocator = Locator.Locator("//p[text()='Organize online life with NordPass — a secure solution for passwords, passkeys, credit cards, and more.']");
            return await descriptionLocator.InnerTextAsync();
        }

        public async Task<List<string>> GetFeaturesAsync()
        {
            var featureElements = await Page.QuerySelectorAllAsync("//div[@id='Hero - homepage']//ul/li");
            var features = new List<string>();
            foreach (var featureElement in featureElements)
            {
                features.Add(await featureElement.InnerTextAsync());
            }
            return features;
        }

        public async Task<ILocator> GetBusinessButtonAsync()
        {
            return Locator.Locator("//a[text()='Business']");
        }

        public async Task<ILocator> GetPersonalButtonAsync()
        {
            return Locator.Locator("//a[text()='Personal']");
        }

        public async Task<ILocator> GetVideoElementAsync()
        {
            return Locator.Locator("//video");
        }
    }
}
