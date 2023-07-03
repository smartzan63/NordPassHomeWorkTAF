namespace NordPassHomeWorkTAF.WebUI.POM.Elements
{
    public class PasskeysBannerHomePageSection : BasePageElement
    {
        private ILocator Locator { get; }
        public PasskeysBannerHomePageSection(IPage page) : base(page, "//div[@id='Passkeys banner - homepage']")
        {
            Locator = GetLocator();
        }

        public async Task<bool> IsVisibleAsync()
        {
            return await Locator.IsVisibleAsync();
        }

        public async Task<string> GetTitleAsync()
        {
            var titleLocator = Locator.Locator("h2");
            return await titleLocator.InnerTextAsync();
        }

        public async Task<string> GetDescriptionAsync()
        {
            var descriptionLocator = Locator.Locator("p");
            return await descriptionLocator.InnerTextAsync();
        }

        public async Task<ILocator> GetLearnMoreButtonAsync()
        {
            return Locator.Locator("//a[text()='Learn more']");
        }
    }

}
