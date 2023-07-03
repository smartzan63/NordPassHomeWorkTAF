namespace NordPassHomeWorkTAF.WebUI.POM.Elements
{
    public class BasePageElement
    {
        protected IPage Page { get; }
        protected string Selector { get; }

        public BasePageElement(IPage page, string selector)
        {
            Page = page;
            Selector = selector;
        }

        protected ILocator GetLocator()
        {
            return Page.Locator(Selector);
        }

        public async Task<bool> IsVisibleAsync()
        {
            var locator = GetLocator();
            return await locator.IsVisibleAsync();
        }

        public async Task WaitUntilIsVisible()
        {
            var locator = GetLocator();
            while (!await locator.IsVisibleAsync())
            {
                await Task.Delay(100); // wait for 100ms before checking again
            }
        }
    }
}