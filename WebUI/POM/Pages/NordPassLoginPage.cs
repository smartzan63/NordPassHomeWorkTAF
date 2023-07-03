using NordPassHomeWorkTAF.WebUI.POM.Elements;

namespace NordPassHomeWorkTAF.WebUI.POM.Pages
{
    public class NordPassLoginPage : BasePage
    {
        public Button CreateAccountButton { get; }
        public Button LogInButton { get; }

        public NordPassLoginPage(IPage page) : base(page)
        {
            CreateAccountButton = new Button(page, "//div[text()='Create Account']");
            LogInButton = new Button(page, "//div[text()='Log In']");
        }
    }
}
