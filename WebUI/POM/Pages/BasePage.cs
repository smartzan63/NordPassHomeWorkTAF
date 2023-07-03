namespace NordPassHomeWorkTAF.WebUI.POM.Pages
{
    public class BasePage
    {
        protected IPage Page { get; }
        public BasePage(IPage page)
        {
            Page = page;
        }

        public virtual async Task WaitUntilLoaded()
        {
        }
    }



}
