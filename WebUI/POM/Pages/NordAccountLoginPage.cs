using NordPassHomeWorkTAF.WebUI.POM.Elements;

namespace NordPassHomeWorkTAF.WebUI.POM.Pages
{
    public class NordAccountLoginPage : BasePage
    {
        public InputField PasswordField { get; }
        public Button LoginButton { get; }

        public NordAccountLoginPage(IPage page) : base(page)
        {
            PasswordField = new InputField(page, "[data-testid='signin-password-input']");
            LoginButton = new Button(page, "[data-testid='signin-button']");
        }
    }
}
