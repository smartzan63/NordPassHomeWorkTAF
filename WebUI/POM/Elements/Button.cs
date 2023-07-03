namespace NordPassHomeWorkTAF.WebUI.POM.Elements
{
    public class Button : BasePageElement
    {
        public Button(IPage page, string selector) : base(page, selector)
        {
        }

        public async Task ClickAsync()
        {
            var locator = GetLocator();
            await locator.ClickAsync();
        }

        public async Task HoverAsync()
        {
            var locator = GetLocator();
            await locator.HoverAsync();
        }

        public async Task<string> GetTextAsync()
        {
            var locator = GetLocator();
            return await locator.InnerTextAsync();
        }
    }
}
