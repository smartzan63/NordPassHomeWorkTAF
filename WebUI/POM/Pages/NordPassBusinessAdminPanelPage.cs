using NordPassHomeWorkTAF.WebUI.POM.Elements;

namespace NordPassHomeWorkTAF.WebUI.POM.Pages
{
    public class NordPassBusinessAdminPanelPage : BasePage
    {
        public InputField EmailField { get; }
        public Button ContinueButton { get; }

        public NordPassBusinessAdminPanelPage(IPage page) : base(page)
        {
            EmailField = new InputField(page, "[data-testid='email-input']");
            ContinueButton = new Button(page, "[data-testid='submit-email-button']");
        }
    }
}
