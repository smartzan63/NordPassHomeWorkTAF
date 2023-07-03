namespace NordPassHomeWorkTAF.WebUI.POM.Elements
{
    public class InputField : BasePageElement
    {
        public InputField(IPage page, string selector) : base(page, selector)
        {
        }

        public async Task EnterTextAsync(string text)
        {
            var locator = GetLocator();
            await locator.FillAsync(text);
        }
    }
}
