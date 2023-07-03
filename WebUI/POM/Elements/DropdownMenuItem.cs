namespace NordPassHomeWorkTAF.WebUI.POM.Elements
{
    public class DropdownMenuItem : BasePageElement
    {
        private IElementHandle OptionElement { get; }
        private IElementHandle HelpTextElement { get; }

        public DropdownMenuItem(IPage page, string selector) : base(page, selector)
        {
            var optionElementSelector = selector + "//span[contains(@class,'whitespace-no-wrap text-base')]";
            var optionHelpTextElement = selector + "//span[contains(@class,'text-small lg:text-nano text-grey-dark block')]";

            OptionElement = page.QuerySelectorAsync(optionElementSelector).Result;
            HelpTextElement = page.QuerySelectorAsync(optionHelpTextElement).Result;
        }

        public async Task<string> GetOptionTextAsync()
        {
            string innerText = await OptionElement.InnerTextAsync();
            return innerText;
        }


        public async Task<string> GetHelpTextAsync()
        {
            string innerText =  await HelpTextElement.InnerTextAsync();
            return innerText;
        }

        public async Task ClickAsync()
        {
            await OptionElement.ClickAsync();
        }
    }

}
