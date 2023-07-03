using NordPassHomeWorkTAF.WebUI.POM.Elements;

namespace NordPassHomeWorkTAF.WebUI.POM.Pages
{
    public class NordPassHomePage : BasePage
    {
        public Button LoginMenuButton { get; }
        public HeaderPageObject Header { get; }
        public HeroHomePageSection HeroHomePageSection { get; }
        public CredibilityHomePageSection CredibilityHomePageSection { get; }
        public PasskeysBannerHomePageSection PasskeysBannerHomePageSection { get; }

        public NordPassHomePage(IPage page) : base(page)
        {
            LoginMenuButton = new Button(page, "//span[contains(@class,'HeaderV2__login-btn w-full')]");
            Header = new HeaderPageObject(page);
            HeroHomePageSection = new HeroHomePageSection(page);
            CredibilityHomePageSection = new CredibilityHomePageSection(page);
            PasskeysBannerHomePageSection = new PasskeysBannerHomePageSection(page);
        }

        public async Task ClickLoginButton()
        {
            await LoginMenuButton.ClickAsync();
        }

        public override async Task WaitUntilLoaded()
        {
            await LoginMenuButton.WaitUntilIsVisible();
        }
    }

}
