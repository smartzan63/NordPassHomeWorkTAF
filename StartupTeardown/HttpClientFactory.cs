using Microsoft.Extensions.Configuration;
using NordPassHomeWorkTAF.Configuration;

namespace NordPassHomeWorkTAF.StartupTeardown
{
    public class HttpClientFactory
    {
        private readonly ApiConfig _apiConfig;
        private readonly String _hostNameKey;

        public HttpClientFactory(IConfiguration configuration, string hostNameKey)
        {
            _apiConfig = new ApiConfig();
            configuration.GetSection("ApiConfig").Bind(_apiConfig);
            _hostNameKey = hostNameKey;
        }

        public HttpClient CreateClient()
        {
            var client = new HttpClient();

            if (_apiConfig.HostNames.ContainsKey(_hostNameKey))
            {
                client.BaseAddress = new Uri(_apiConfig.HostNames[_hostNameKey]);
            }

            return client;
        }
    }

}
