@WebUi
Feature: First challenge - UI Automation
    As a user
    I want to open NordPass.com and see correct page
    So I can check available Features, Pricing, Blog and Help
    Also I want to see that Log In options are available


Scenario: Login options are avaiable
    When I open page by key 'NordPassHomePage'
    And Hover over Login button
    Then Login options are availabe
    | Option                      | Help Text                                              |
    | Access my passwords         | View and manage passwords in a web browser             |
    | Manage my subscription      | View, upgrade or cancel my Nord Security subscriptions |
    | Access Business Admin Panel | Manage my organization’s account and its members       |
    When I click on 'Access my passwords'
    Then NordPass login page is displayed
    When I open the first tab
    And Hover over Login button
    When I click on 'Manage my subscription'
    Then NordAccount login page is displayed
    When I open the first tab
    And Hover over Login button
    When I click on 'Access Business Admin Panel'
    Then NordPass Business Admin Panel is displayed

Scenario: Main page checks
    When I open page by key 'NordPassHomePage'
    Then Main page have correct header
    And Main page body contains section 'Hero - homepage'
    And Main page body contains section 'Credibility - homepage'
    And Main page body contains section 'Passkeys banner - homepage'

    #More checks can be implemented, but it takes time
    #And Main page body contains section 'Every day with NordPass - homepage'
    #And Main page body contains section 'Autofill passwords, credit cards, and more - homepage'
    #And Main page body contains section 'Share securely - homepage'
    #And Main page body contains section 'Family Banner - homepage'
    #And Main page body contains section 'Single solution. Multiple use cases - homepage'
    #And Main page body contains section 'Business and Personal plans cards - homepage'
    #And Main page body contains section 'What our customers say - homepage'
    #And Main page body contains section 'Advanced security - homepage'
    #And Main page body contains section 'Why NordPass is better than browser password managers - homepage'
    #And Main page body contains section 'Footer'
    #And Main page body contains section 'cookie-consent'