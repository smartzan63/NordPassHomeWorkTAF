namespace NordPassHomeWorkTAF.StepDefinitions
{
    [Binding]
    public class WebUISteps
    {
        private readonly UITestContext _uiTestContext;

        public WebUISteps(UITestContext uiTestContext)
        {
            _uiTestContext = uiTestContext;
        }

        [StepDefinition(@"I open page by key '([^']*)'")]
        public async Task WhenIOpenPageByKey(string pageKey)
        {
            await _uiTestContext.OpenPageByKey(pageKey);
        }

        [StepDefinition(@"Click on Login button")]
        public async Task WhenClickOnLoginButton()
        {
            await _uiTestContext.ClickOnLoginButton();
        }

        [StepDefinition(@"Hover over Login button")]
        public async Task WhenHoverOverLoginButton()
        {
            await _uiTestContext.HoverOverLoginButton();
        }

        [StepDefinition(@"Login options are availabe")]
        public async Task WhenLoginOptionsAreAvailabe(Table table)
        {
            await _uiTestContext.CheckLoginOptions(table);
        }

        [StepDefinition(@"I click on '([^']*)'")]
        public async Task WhenIClickOn(string itemName)
        {
            await _uiTestContext.ClickOnMenuItem(itemName);
        }

        [StepDefinition(@"NordPass login page is displayed")]
        public async Task ThenNordPassLoginPageIsDisplayed()
        {
            await _uiTestContext.CheckNordPassLoginPageIsDisplayed();
        }

        [StepDefinition(@"I open the first tab")]
        public async Task WhenIOpenTheFirstTab()
        {
            await _uiTestContext.OpenFirstTab();
        }

        [StepDefinition(@"NordAccount login page is displayed")]
        public async Task ThenNordAccountLoginPageIsDisplayed()
        {
            await _uiTestContext.CheckNordAccountLoginPageIsDisplayed();
        }

        [StepDefinition(@"NordPass Business Admin Panel is displayed")]
        public async Task ThenNordPassBusinessAdminPanelIsDisplayed()
        {
            await _uiTestContext.CheckNordPassBusinessAdminPanelPage();
        }

        [StepDefinition(@"Main page have correct header")]
        public async Task MainPageHaveCorrectHeader()
        {
            await _uiTestContext.CheckMainPageHeader();
        }

        [StepDefinition(@"Main page body contains section '([^']*)'")]
        public async Task ThenMainPageBodyContainsSection(string sectionId)
        {
            await _uiTestContext.CheckMainPageBodySection(sectionId);
        }
    }
}
