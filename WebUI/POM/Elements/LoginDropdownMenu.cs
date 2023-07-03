namespace NordPassHomeWorkTAF.WebUI.POM.Elements
{
    public class LoginDropdownMenu : BasePageElement
    {
        public static readonly string Selector = "//div[@class='HeaderV2__login-menu secondary-menu-enter-done']";

        public LoginDropdownMenu(IPage page) : base(page, Selector)
        {
        }

        public async Task<List<DropdownMenuItem>> GetMenuItemsAsync()
        {
            var menuItems = new List<DropdownMenuItem>();
            var elements = await Page.QuerySelectorAllAsync(Selector + "//li[a]");
            for (int i = 0; i < elements.Count; i++)
            {
                var menuItem = new DropdownMenuItem(Page, $"{Selector}//li[a][{i + 1}]");
                menuItems.Add(menuItem);
            }
            return menuItems;
        }

    }
}

