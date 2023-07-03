using BoDi;
using Microsoft.Extensions.Configuration;
using NordPassHomeWorkTAF.Contexts;
using NordPassHomeWorkTAF.Configuration;
using NordPassHomeWorkTAF.WebUI.POM;
using NordPassHomeWorkTAF.Common;
using Microsoft.Extensions.Logging;

namespace NordPassHomeWorkTAF.StartupTeardown.Specflow
{
    [Binding]
    public sealed class SpecFlowHooks
    {
        private readonly IObjectContainer _objectContainer;
        private readonly IConfiguration _configuration;
        private readonly ScenarioContext _scenarioContext;
        private readonly ILogger _logger;

        public SpecFlowHooks(IObjectContainer objectContainer,
                             IConfiguration configuration,
                             ScenarioContext scenarioContext,
                             ILogger logger)
        {
            _objectContainer = objectContainer;
            _configuration = configuration;
            _scenarioContext = scenarioContext;
            _logger = logger;
        }

        [BeforeScenario("NordpassTestHostAPI", Order = 1)]
        public void FirstBeforeScenarioForService1()
        {
            var interTestDataContext = new InterTestDataContext();
            _objectContainer.RegisterInstanceAs(interTestDataContext);

            var nordpassHttpClientContext = new NordpassHttpClientContext(_configuration, interTestDataContext, _scenarioContext, _logger);
            _objectContainer.RegisterInstanceAs(nordpassHttpClientContext, "NordpassHttpClientContext");

            var apiConfig = _objectContainer.Resolve<ApiConfig>();

            _scenarioContext["NordpassTestHostApiConfig"] = apiConfig;
        }

        [BeforeScenario("WebUi")]
        public async Task WebUiStartup()
        {
            var browser = new Browser();
            await browser.InitializeAsync();
            _objectContainer.RegisterInstanceAs(browser);

            var browserContext = new TestBrowserContext(_logger);
            browserContext.Browser = browser;
            browserContext.PlaywrightBrowserContext = browser.PlaywrightBrowserContext;

            var webUiConfig = _objectContainer.Resolve<WebUiConfig>();
            browserContext.Endpoints = webUiConfig.Endpoints;

            _objectContainer.RegisterInstanceAs(browserContext);

            var interTestDataContext = new InterTestDataContext();
            _objectContainer.RegisterInstanceAs(interTestDataContext);
        }

        [AfterScenario("WebUi")]
        public async Task WebUiTeardown()
        {
            var browserContext = _objectContainer.Resolve<TestBrowserContext>();
            await browserContext.Browser.PlaywrightBrowser.CloseAsync();
        }

        [BeforeTestRun]
        public static void BeforeTestRun(IObjectContainer container)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("Program", LogLevel.Debug)
                    .AddProvider(new NUnitLoggerProvider());
            });

            ILogger logger = loggerFactory.CreateLogger<SpecFlowHooks>();

            container.RegisterInstanceAs<ILogger>(logger);
            container.RegisterInstanceAs<string>("NordpassTestHost", "hostNameKey");

            var configurationRoot = new ConfigurationBuilder()
                .AddJsonFile("Configuration//appsettings.json")
                .AddEnvironmentVariables() //To get credentials from Environment we should use variable names like that: Credentials__TestApiCredentials__Username,
                                           //meaning source file to bind SomeConfigSection__CredentialsClassName__FieldName
                .Build();

            container.RegisterInstanceAs<IConfiguration>(configurationRoot);
            var credentials = configurationRoot.GetSection("Credentials:TestApiCredentials").Get<TestApiCredentials>();

            var apiConfig = CreateAndRegisterConfig<ApiConfig>(container, configurationRoot, "ApiConfig");
            var webUiConfig = CreateAndRegisterConfig<WebUiConfig>(container, configurationRoot, "WebUiConfig");
            container.RegisterInstanceAs<TestApiCredentials>(credentials);

            var jsonResourceContext = new JsonResourceContext();
            container.RegisterInstanceAs(jsonResourceContext);

        }

        private static T CreateAndRegisterConfig<T>(IObjectContainer container, IConfiguration configuration, string sectionName) where T : class, new()
        {
            var config = new T();
            configuration.GetSection(sectionName).Bind(config);

            var replacements = new Dictionary<string, string>();

            var properties = config.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(Dictionary<string, string>))
                {
                    var stringDictionary = (Dictionary<string, string>)property.GetValue(config);
                    foreach (var kvp in stringDictionary)
                    {
                        replacements[kvp.Key] = kvp.Value;
                    }
                }
            }

            ResolvePlaceholdersInConfig(config, replacements);

            container.RegisterInstanceAs(config);

            return config;
        }

        private static void ResolvePlaceholdersInConfig<T>(T config, Dictionary<string, string> replacements)
        {
            var properties = config.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.GetIndexParameters().Length == 0)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        var value = (string)property.GetValue(config);
                        value = PlaceholderResolver.ResolvePlaceholders(new Dictionary<string, string> { { property.Name, value } }, replacements)[property.Name];
                        property.SetValue(config, value);
                    }
                    else if (property.PropertyType == typeof(Dictionary<string, string>))
                    {
                        var stringDictionary = (Dictionary<string, string>)property.GetValue(config);
                        var resolvedDictionary = PlaceholderResolver.ResolvePlaceholders(stringDictionary, replacements);

                        property.SetValue(config, resolvedDictionary);
                    }
                    else if (property.PropertyType == typeof(Dictionary<string, Uri>))
                    {
                        var uriDictionary = (Dictionary<string, Uri>)property.GetValue(config);
                        var stringDictionary = uriDictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
                        var resolvedDictionary = PlaceholderResolver.ResolvePlaceholders(stringDictionary, replacements);
                        var uriDictionaryResolved = resolvedDictionary.ToDictionary(kvp => kvp.Key, kvp => new Uri(kvp.Value));

                        property.SetValue(config, uriDictionaryResolved);
                    }
                }
            }
        }
    }
}
