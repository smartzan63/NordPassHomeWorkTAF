using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NordPassHomeWorkTAF.Common;
using NordPassHomeWorkTAF.Configuration;
using System;

namespace NordPassHomeWorkTAF.Contexts
{
    public class NordpassHttpClientContext : HttpClientContext
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly ILogger _logger;

        public ApiConfig ApiConfig => (ApiConfig)_scenarioContext["NordpassTestHostApiConfig"];

        public NordpassHttpClientContext(IConfiguration configuration, InterTestDataContext interTestDataContext, ScenarioContext scenarioContext, ILogger logger)
            : base(configuration, interTestDataContext, "NordpassTestHost", logger)
        {
            _scenarioContext = scenarioContext;
            _logger = logger;
        }

        public Uri GetRequestUri(string endpointName)
        {
            var apiConfig = ApiConfig;

            // Get endpoint from ApiConfig
            if (!apiConfig.Endpoints.TryGetValue(endpointName, out var endpoint))
            {
                _logger.LogError($"Endpoint '{endpointName}' does not exist in the configuration.");
                throw new Exception($"Endpoint '{endpointName}' does not exist in the configuration.");
            }

            return new Uri(HttpClient.BaseAddress, endpoint.PathAndQuery);
        }
    }
}
