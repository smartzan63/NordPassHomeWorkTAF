namespace NordPassHomeWorkTAF.WebUI.POM.Elements
{
public class HeaderPageObject : BasePageElement
{
    public HeaderPageObject(IPage page) : base(page, "header")
    {
    }

    public async Task<IElementHandle> GetLogoElementAsync()
    {
        return await Page.QuerySelectorAsync("a[aria-label='logo']");
    }

    public async Task<IReadOnlyCollection<IElementHandle>> GetNavItemsAsync()
    {
        return await Page.QuerySelectorAllAsync("ul.HeaderV2__nav > li");
    }
}
}
