using NordPassHomeWorkTAF.Contexts;
using NordPassHomeWorkTAF.WebUI.POM.Elements;
using NordPassHomeWorkTAF.WebUI.POM.Pages;

public class UITestContext
{
    private readonly TestBrowserContext _browserContext;

    public UITestContext(TestBrowserContext browserContext)
    {
        _browserContext = browserContext;
    }

    public async Task OpenPageByKey(string pageKey)
    {
        var uri = _browserContext.Endpoints[pageKey];
        var url = uri.AbsoluteUri;
        await _browserContext.Browser.GoToUrlAsync(url);
        var homePage = new NordPassHomePage(_browserContext.Browser.Page);
        await homePage.WaitUntilLoaded();
    }

    public async Task ClickOnLoginButton()
    {
        var page = new NordPassHomePage(_browserContext.Browser.Page);
        await page.LoginMenuButton.ClickAsync();
    }

    public async Task HoverOverLoginButton()
    {
        var page = new NordPassHomePage(_browserContext.Browser.Page);
        await page.LoginMenuButton.HoverAsync();
        var dropdownMenu = new LoginDropdownMenu(_browserContext.Browser.Page);
        await _browserContext.Browser.Page.WaitForSelectorAsync(LoginDropdownMenu.Selector);
    }

    public async Task CheckLoginOptions(Table table)
    {
        var dropdownMenu = new LoginDropdownMenu(_browserContext.Browser.Page);
        var menuItems = await dropdownMenu.GetMenuItemsAsync();

        foreach (var row in table.Rows)
        {
            var option = row["Option"];
            var helpText = row["Help Text"];

            DropdownMenuItem menuItem = null;
            foreach (var item in menuItems)
            {
                if ((await item.GetOptionTextAsync()) == option && (await item.GetHelpTextAsync()) == helpText)
                {
                    menuItem = item;
                    break;
                }
            }

            if (menuItem == null)
            {
                throw new Exception($"Menu item '{option}' with help text '{helpText}' not found in the dropdown menu.");
            }
            else
            {
                // Remove the matched item from the list to avoid checking it again in the next iteration
                menuItems.Remove(menuItem);
            }
        }
    }

    public async Task OpenFirstTab()
    {
        var firstTab = _browserContext.Pages.First();
        await firstTab.BringToFrontAsync();
    }

    public async Task ClickOnMenuItem(string menuItemName)
    {
        var dropdownMenu = new LoginDropdownMenu(_browserContext.Browser.Page);
        var menuItems = await dropdownMenu.GetMenuItemsAsync();
        var menuTexts = new List<string>();
        foreach (var item in menuItems)
        {
            menuTexts.Add(await item.GetOptionTextAsync());
        }
        int index = Array.FindIndex(menuTexts.ToArray(), text => text.Contains(menuItemName));
        if (index == -1)
        {
            throw new Exception($"Menu item '{menuItemName}' not found in the dropdown menu.");
        }
        var menuItem = menuItems[index];

        IPage newPage = null;
        _browserContext.PlaywrightBrowserContext.Page += (_, page) => newPage = page;

        await menuItem.ClickAsync();

        int attempts = 0;

        while (newPage == null && attempts < 10)
        {
            await Task.Delay(500);
            attempts++;
        }

        // If a new page has not been opened, assume the current page has been navigated
        if (newPage == null)
        {
            newPage = _browserContext.Browser.Page;
        }

        await newPage.WaitForLoadStateAsync(LoadState.Load);
    }

    public async Task CheckNordPassLoginPageIsDisplayed()
    {
        var lastPage = _browserContext.Pages.Last();
        await lastPage.BringToFrontAsync();
        var nordPassLoginPage = new NordPassLoginPage(lastPage);
        await nordPassLoginPage.CreateAccountButton.IsVisibleAsync();
        await nordPassLoginPage.LogInButton.IsVisibleAsync();
    }

    public async Task CheckNordAccountLoginPageIsDisplayed()
    {
        var lastPage = _browserContext.Pages.Last();
        await lastPage.BringToFrontAsync();
        var nordAccountLoginPage = new NordAccountLoginPage(lastPage);
        await nordAccountLoginPage.PasswordField.IsVisibleAsync();
        await nordAccountLoginPage.LoginButton.IsVisibleAsync();
    }

    public async Task CheckNordPassBusinessAdminPanelPage()
    {
        var businessAdminPanelPage = new NordPassBusinessAdminPanelPage(_browserContext.Browser.Page);
        await businessAdminPanelPage.EmailField.IsVisibleAsync();
        await businessAdminPanelPage.ContinueButton.IsVisibleAsync();
    }

    public async Task CheckMainPageHeader()
    {
        var homePage = new NordPassHomePage(_browserContext.Browser.Page);
        await homePage.Header.WaitUntilIsVisible();

        var headerElement = homePage.Header;
        (await headerElement.IsVisibleAsync()).Should().BeTrue("because the header element should exist on the page");

        var logoElement = await homePage.Header.GetLogoElementAsync();
        (await logoElement.IsVisibleAsync()).Should().BeTrue("because the logo element should exist in the header");

        var navItems = await homePage.Header.GetNavItemsAsync();
        navItems.Count.Should().Be(6, "because there should be 6 navigation items in the header");

        var expectedNavItems = new List<string> { "PERSONAL", "BUSINESS", "Features", "Pricing", "Blog", "Help" };
        for (int i = 0; i < navItems.Count; i++)
        {
            var navItemText = await navItems.ElementAt(i).InnerTextAsync();
            navItemText.Should().Be(expectedNavItems[i], $"because the text for navigation item {i + 1} should be {expectedNavItems[i]}");
        }
    }

    public async Task CheckMainPageBodySection(string sectionId)
    {
        var homePage = new NordPassHomePage(_browserContext.Browser.Page);

        switch (sectionId)
        {
            case "Hero - homepage":
                await CheckHeroHomePageSection(homePage);
                break;
            case "Credibility - homepage":
                await CheckCredibilityHomePageSection(homePage);
                break;
            case "Passkeys banner - homepage":
                await CheckPasskeysBannerHomePageSection(homePage);
                break;
            default:
                throw new Exception($"Unknown section id: {sectionId}");
        }
    }

    private async Task CheckHeroHomePageSection(NordPassHomePage homePage)
    {
        var heroSection = homePage.HeroHomePageSection;
        await heroSection.WaitUntilIsVisible();

        var heroSectionElement = heroSection;
        (await heroSectionElement.IsVisibleAsync()).Should().BeTrue("because the Hero section should exist on the page");

        var title = await heroSection.GetTitleAsync();
        title.Should().Be("NordPass — your digital life manager", "because that is the expected title");

        var description = await heroSection.GetDescriptionAsync();
        description.Should().Be("Organize online life with NordPass — a secure solution for passwords, passkeys, credit cards, and more.", "because that is the expected description");

        var features = await heroSection.GetFeaturesAsync();
        features.Should().BeEquivalentTo(new List<string> { "Generate strong passwords.", "Securely share passwords with co-workers.", "Find out if your data has been breached." }, "because these are the expected features");

        var businessButton = await heroSection.GetBusinessButtonAsync();
        (await businessButton.IsVisibleAsync()).Should().BeTrue("because the Business button should exist in the Hero section");
        (await businessButton.InnerTextAsync()).Should().Be("Business", "because that is the expected text for the Business button");

        var personalButton = await heroSection.GetPersonalButtonAsync();
        (await personalButton.IsVisibleAsync()).Should().BeTrue("because the Personal button should exist in the Hero section");
        (await personalButton.InnerTextAsync()).Should().Be("Personal", "because that is the expected text for the Personal button");

        var videoElement = await heroSection.GetVideoElementAsync();
        (await videoElement.IsVisibleAsync()).Should().BeTrue("because the video element should exist in the Hero section");
    }

    private async Task CheckCredibilityHomePageSection(NordPassHomePage homePage)
    {
        var credibilitySection = homePage.CredibilityHomePageSection;
        await credibilitySection.WaitUntilIsVisible();

        var credibilitySectionElement = credibilitySection;
        (await credibilitySectionElement.IsVisibleAsync()).Should().BeTrue("because the Credibility section should exist on the page");

        var businessClients = await credibilitySection.GetBusinessClientsAsync();
        businessClients.Should().Be("Business clients", "because that is the expected number of business clients");

        var mediaPresence = await credibilitySection.GetMediaPresenceAsync();
        mediaPresence.Should().Be("Media presence around the world", "because that is the expected media presence");

        var trustpilotRating = await credibilitySection.GetTrustpilotRatingAsync();
        trustpilotRating.Should().Be("Trustpilot Rating", "because that is the expected Trustpilot rating");

        var usersWorldwide = await credibilitySection.GetUsersWorldwideAsync();
        usersWorldwide.Should().Be("Users worldwide", "because that is the expected number of users worldwide");
    }

    private async Task CheckPasskeysBannerHomePageSection(NordPassHomePage homePage)
    {
        var passkeysBannerSection = homePage.PasskeysBannerHomePageSection;
        await passkeysBannerSection.WaitUntilIsVisible();

        var passkeysBannerSectionElement = passkeysBannerSection;
        (await passkeysBannerSectionElement.IsVisibleAsync()).Should().BeTrue("because the Passkeys Banner section should exist on the page");

        var title = await passkeysBannerSection.GetTitleAsync();
        title.Should().Be("Security simplified: use passkeys with NordPass", "because that is the expected title");

        var description = await passkeysBannerSection.GetDescriptionAsync();
        description.Should().Be("NordPass now supports passkeys, a new passwordless authentication standard that is more secure and convenient to use than traditional passwords.", "because that is the expected description");

        var learnMoreButton = await passkeysBannerSection.GetLearnMoreButtonAsync();
        (await learnMoreButton.IsVisibleAsync()).Should().BeTrue("because the Learn More button should exist in the Passkeys Banner section");
        (await learnMoreButton.InnerTextAsync()).Should().Be("Learn more", "because that is the expected text for the Learn More button");
    }
}
