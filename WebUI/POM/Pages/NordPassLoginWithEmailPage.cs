using NordPassHomeWorkTAF.WebUI.POM.Elements;

namespace NordPassHomeWorkTAF.WebUI.POM.Pages
{
    public class NordPassLoginWithEmailPage : BasePage
    {
        public InputField EmailField { get; }
        public Button ContinueButton { get; }

        public NordPassLoginWithEmailPage(IPage page) : base(page)
        {
            EmailField = new InputField(page, "[data-testid='email']");
            ContinueButton = new Button(page, "button[type='submit']");
        }
    }
}
