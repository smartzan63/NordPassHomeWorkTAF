using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NordPassHomeWorkTAF.Common;
using NordPassHomeWorkTAF.StartupTeardown;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace NordPassHomeWorkTAF.Contexts
{
    public class HttpClientContext
    {
        public HttpClient HttpClient { get; }
        private readonly InterTestDataContext _interTestDataContext;
        private readonly ILogger _logger;

        public HttpClientContext(IConfiguration configuration, InterTestDataContext interTestDataContext, string hostNameKey, ILogger logger)
        {
            var httpClientFactory = new HttpClientFactory(configuration, hostNameKey);
            _interTestDataContext = interTestDataContext;
            HttpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public async Task<HttpResponseMessage> SendHttpRequestAsync(HttpMethod method, Uri uri, HttpContent content = null, string token = null, HttpAuthorizationType authType = HttpAuthorizationType.None)
        {
            var request = new HttpRequestMessage(method, uri)
            {
                Content = content
            };

            switch (authType)
            {
                case HttpAuthorizationType.Bearer:
                    if (!string.IsNullOrEmpty(token))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    break;
                case HttpAuthorizationType.Hmac:
                    ConstructHmacAuthorizationHeaders(method.Method, uri.AbsolutePath);
                    request.Headers.Add("X-Nonce", _interTestDataContext.Nonce);
                    request.Headers.Add("X-Signature", _interTestDataContext.HmacSignature);
                    break;
                case HttpAuthorizationType.None:
                    // Do nothing
                    break;
                default:
                    throw new ArgumentException($"Unsupported authorization type: {authType}");
            }

            _logger.LogInformation($"Sending {method} request to {uri}");
            _logger.LogInformation($"Request Headers: {request.Headers.ToString()}");

            if (content != null)
            {
                var requestBody = await content.ReadAsStringAsync();
                _logger.LogInformation($"Request Body: {requestBody}");
            }

            HttpResponseMessage response;
            try
            {
                response = await HttpClient.SendAsync(request);
                _logger.LogInformation($"Received response with status code {response.StatusCode}");
                _logger.LogInformation($"Response Headers: {response.Headers.ToString()}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending HTTP request");
                throw;
            }

            return response;
        }

        public async void StoreResponse(HttpResponseMessage httpResponse)
        {
            var content = await httpResponse.Content.ReadAsStringAsync();
            _interTestDataContext.LastResponse = new HttpResponse
            {
                StatusCode = httpResponse.StatusCode,
                Content = content,
                Headers = httpResponse.Headers
            };

            _logger.LogInformation($"Stored response: {content}");
        }

        public void ValidateStatusCode(string httpStatusCode)
        {
            HttpStatusCode expectedStatusCode = Enum.Parse<HttpStatusCode>(httpStatusCode);

            _interTestDataContext.LastResponse.Should().NotBeNull();
            _interTestDataContext.LastResponse.StatusCode.Should().Be(expectedStatusCode);

            if (_interTestDataContext.LastResponse.StatusCode != expectedStatusCode)
            {
                _logger.LogWarning($"Expected status code {expectedStatusCode}, but got {_interTestDataContext.LastResponse.StatusCode}");
            }
        }

        private void ConstructHmacAuthorizationHeaders(string httpMethod, string requestPath)
        {
            _logger.LogInformation("Constructing HMAC Authorization headers.");

            var signatureKey = _interTestDataContext.SignatureKey;
            var nonce = _interTestDataContext.Nonce;

            var requestData = $"{httpMethod} {requestPath}";

            // Generate the HMAC signature
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(signatureKey)))
            {
                var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(requestData));
                var signatureHex = BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();
                _interTestDataContext.HmacSignature = signatureHex;
                _interTestDataContext.Nonce = nonce;
            }

            _logger.LogInformation($"Constructed HmacSignature with value: {_interTestDataContext.HmacSignature}");
        }
    }

}
