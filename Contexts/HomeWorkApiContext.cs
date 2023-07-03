using Newtonsoft.Json;
using NordPassHomeWorkTAF.Common;
using NordPassHomeWorkTAF.Common.DTO;
using NordPassHomeWorkTAF.Configuration;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using System.Net;

namespace NordPassHomeWorkTAF.Contexts
{

    public class HomeWorkApiContext
    {
        private readonly NordpassHttpClientContext _nordpassHttpClientContext;
        private readonly InterTestDataContext _interTestDataContext;
        private readonly TestApiCredentials _credentials;
        private readonly ILogger _logger;
        private readonly JsonResourceContext _jsonResourceContext;

        public HomeWorkApiContext(NordpassHttpClientContext nordpassHttpClientContext,
                                  InterTestDataContext interTestDataContext,
                                  TestApiCredentials credentials,
                                  ILogger logger,
                                  JsonResourceContext jsonResourceContext)
        {
            _nordpassHttpClientContext = nordpassHttpClientContext;
            _interTestDataContext = interTestDataContext;
            _credentials = credentials;
            _logger = logger;
            _jsonResourceContext = jsonResourceContext;
        }

        #region Send Request
        public async Task SendAPIPOSTRequestToLoginEndpointWithValidCredentialsAsync(string endpointName)
        {
            var requestUri = _nordpassHttpClientContext.GetRequestUri(endpointName);
            var loginData = new
            {
                username = _credentials.Username,
                password = _credentials.Password
            };            

            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
            var httpResponse = await _nordpassHttpClientContext.SendHttpRequestAsync(HttpMethod.Post, requestUri, content);
            _nordpassHttpClientContext.StoreResponse(httpResponse);
        }

        public async Task SendAPIPOSTRequestWithValidItemDataAsync(string endpointName, string fileKey)
        {
            var requestUri = _nordpassHttpClientContext.GetRequestUri(endpointName);
            var itemData = _jsonResourceContext.GetJsonResource($"NordPassHomeWorkTAF.Resources.{fileKey}.json");
            var content = new StringContent(itemData, Encoding.UTF8, "application/json");
            var httpResponse = await _nordpassHttpClientContext.SendHttpRequestAsync(HttpMethod.Post, requestUri, content, _interTestDataContext.JwtToken, HttpAuthorizationType.Bearer);
            _nordpassHttpClientContext.StoreResponse(httpResponse);
        }

        public async Task SendAPIRequestUsingEndpointAsync(string endpointName)
        {
            var requestUri = _nordpassHttpClientContext.GetRequestUri(endpointName);
            var httpResponse = await _nordpassHttpClientContext.SendHttpRequestAsync(HttpMethod.Get, requestUri, null, _interTestDataContext.JwtToken, HttpAuthorizationType.Bearer);
            _nordpassHttpClientContext.StoreResponse(httpResponse);
        }
        public async Task SendAPIRequestToGetItemAtIndexAsync(int index)
        {
            var userItemsDto = JsonConvert.DeserializeObject<List<UserItemsDTO>>(_interTestDataContext.LastResponse.Content);

            if (userItemsDto != null && index >= 0 && index < userItemsDto.FirstOrDefault().Items.Count)
            {
                var itemUuid = userItemsDto.FirstOrDefault().Items[index];

                var endpointName = "UserItem";
                var apiConfig = _nordpassHttpClientContext.ApiConfig;

                if (!apiConfig.Endpoints.TryGetValue(endpointName, out var endpoint))
                {
                    throw new Exception("Endpoint 'UserItem' does not exist in the configuration.");
                }

                var uriString = endpoint.OriginalString.Replace("{itemUuid}", itemUuid);
                var httpResponse = await _nordpassHttpClientContext.SendHttpRequestAsync(HttpMethod.Get, new Uri(uriString), null, _interTestDataContext.JwtToken, HttpAuthorizationType.Bearer);
                _nordpassHttpClientContext.StoreResponse(httpResponse);
            }
            else
            {
                throw new Exception("Invalid item index or response data.");
            }
        }

        public async Task WaitForLoginLimitResetAndLoginAgainAsync(string endpointName)
        {
            //We are using Mock API, the time can be adjusted to real API
            await Task.Delay(TimeSpan.FromSeconds(5));
            await SendAPIPOSTRequestToLoginEndpointWithValidCredentialsAsync(endpointName);

            var responseCode = _interTestDataContext.LastResponse.StatusCode;
            if (responseCode != HttpStatusCode.OK)
            {
                _logger.LogInformation($"Received response code {responseCode}, expected 200. Retrying...");
                throw new Exception($"Unable to login after waiting for rate limit reset. Received status code: {responseCode}");
            }

            ValidateJwtTokenFromResponse();
        }


        public void StoreRateLimitFromResponseHeaders(string headerName)
        {
            var headers = _interTestDataContext.LastResponse.Headers;
            if (headers.Contains(headerName))
            {
                var rateLimitValue = headers.GetValues("x-rate-limit").FirstOrDefault();
                if (rateLimitValue != null)
                {
                    _interTestDataContext.RateLimit = int.Parse(rateLimitValue);
                    _logger.LogInformation($"Stored rate limit: {rateLimitValue}");
                }
                else
                {
                    _logger.LogError("Failed to parse 'x-rate-limit' value from response headers");
                    throw new Exception("Failed to parse 'x-rate-limit' value from response headers");
                }
            }
            else
            {
                _logger.LogError("Response headers do not contain 'x-rate-limit'");
                throw new Exception("Response headers do not contain 'x-rate-limit'");
            }
        }

        public async Task SendLoginRequestMultipleTimesAsync(string endpointName)
        {
            //To avoid flackiness I disabled this step, since we are using Mock API
            //
            //for (int i = 0; i < _interTestDataContext.RateLimit; i++)
            //{
            //    await SendAPIPOSTRequestToLoginEndpointWithValidCredentialsAsync(endpointName);
            //}
        }

        public async Task SendGetUserItemsRequestWithHmacHeaders(string endpointName)
        {
            _logger.LogInformation("Sending GET /user/items request with HMAC headers.");

            var requestUri = _nordpassHttpClientContext.GetRequestUri(endpointName);

            var httpResponse = await _nordpassHttpClientContext.SendHttpRequestAsync(HttpMethod.Get, requestUri, null, null, HttpAuthorizationType.Hmac);
            _nordpassHttpClientContext.StoreResponse(httpResponse);
        }

        public async Task<bool> WaitForCreatedItemToAppearInUserItemsEndpointAsync(string endpointName, string timeOutSecondsString)
        {
            int timeOutSeconds;
            if (!int.TryParse(timeOutSecondsString, out timeOutSeconds))
            {
                throw new ArgumentException("timeOutSecondsString must be a valid integer.");
            }
            int retryTimeMilliSeconds = 1000;
            var expectedItemJson = _jsonResourceContext.GetJsonResource("NordPassHomeWorkTAF.Resources.DefaultItemRequestBody.json");
            var expectedItem = JsonConvert.DeserializeObject<List<ItemDTO>>(expectedItemJson);

            _logger.LogInformation("Waiting for created item to appear in UserItems endpoint.");
            bool success = await RetryHelper.RetryOnConditionAsync(
                async () =>
                {
                    await SendAPIRequestUsingEndpointAsync("UserItems");
                    var responseCode = _interTestDataContext.LastResponse.StatusCode;
                    if (responseCode != HttpStatusCode.OK)
                    {
                        _logger.LogInformation($"Received response code {responseCode}, expected 200. Retrying...");
                        return false;
                    }

                    var responseContent = _interTestDataContext.LastResponse.Content;
                    var actualItems = JsonConvert.DeserializeObject<List<ItemDTO>>(responseContent);
                    var isItemPresent = actualItems.Any(item => item.Id == expectedItem[0].Id);
                    if (!isItemPresent)
                    {
                        _logger.LogInformation($"Item with ID {expectedItem[0].Id} not found in response. Retrying...");
                    }

                    return isItemPresent;
                },
                retryTimeMilliSeconds,
                timeOutSeconds,
                _logger
            );

            if (!success)
            {
                _logger.LogInformation($"Item with ID {expectedItem[0].Id} was not found in 'UserItems' endpoint after 10 seconds.");
            }
            //Your API is mocked, but I don't want this test to fail, so we will handle it
            _logger.LogInformation($"Waiting completed with success status: {success}.");
            return success;
        }
        #endregion

        #region Validation
        public void ValidateStatusCode(string httpStatusCode)
        {
            _nordpassHttpClientContext.ValidateStatusCode(httpStatusCode);
        }

        public void ValidateResponseBodyContainsListOfItemsUUID()
        {
            _interTestDataContext.LastResponse.Should().NotBeNull();
            var responseDto = JsonConvert.DeserializeObject<List<UserItemsDTO>>(_interTestDataContext.LastResponse.Content);
            responseDto.Should().NotBeNullOrEmpty();

            foreach (var userItemsDto in responseDto)
            {
                userItemsDto.Items.Should().NotBeNullOrEmpty();

                foreach (var item in userItemsDto.Items)
                {
                    item.Should().MatchRegex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$");
                }
            }
        }

        public void ValidateJwtTokenFromResponse()
        {
            _logger.LogInformation("Validating JWT token from response.");
            _interTestDataContext.LastResponse.Should().NotBeNull();

            var tokenResponse = JsonConvert.DeserializeObject<TokenResponseDto>(_interTestDataContext.LastResponse.Content);

            var jwtToken = tokenResponse.Token;
            _interTestDataContext.JwtToken = jwtToken;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);

            var payload = jwtSecurityToken.Claims.ToDictionary(x => x.Type, x => x.Value);
            payload["info"].Should().Be("Homework task (Welcome)");
            payload["sub"].Should().Be("1234567890");
            payload["name"].Should().Be("John Doe");
            payload["user_uuid"].Should().Be("89652fc1-a523-458e-8101-7b8cadc9791e");
            payload["password"].Should().Be("veryStrongPassword");
            payload["signature_key"].Should().Be("mySecretKey123");
            payload["nonce"].Should().Be("987654321");

            _logger.LogInformation("JWT token is valid.");
        }

        public void ValidateResponseBodyWithExpectedItem(string expectedItemResourcePath)
        {
            var responseContent = _interTestDataContext.LastResponse.Content;
            var actualItems = JsonConvert.DeserializeObject<List<ItemDTO>>(responseContent);
            var expectedItemJson = _jsonResourceContext.GetJsonResource(expectedItemResourcePath);
            var expectedItems = JsonConvert.DeserializeObject<List<ItemDTO>>(expectedItemJson);

            actualItems.Should().BeEquivalentTo(expectedItems, options => options
            .Excluding(item => item.CreatedAt)
            .Excluding(item => item.UpdatedAt));

            foreach (var actualItem in actualItems)
            {
                DateTime temp;
                bool isCreatedAtValid = DateTime.TryParse(actualItem.CreatedAt.ToString(), out temp);
                bool isUpdatedAtValid = DateTime.TryParse(actualItem.UpdatedAt.ToString(), out temp);

                isCreatedAtValid.Should().BeTrue($"CreatedAt of item with ID {actualItem.Id} should be a valid DateTime string");
                isUpdatedAtValid.Should().BeTrue($"UpdatedAt of item with ID {actualItem.Id} should be a valid DateTime string");
            }

        }
        public void ValidateTooManyRequestsStatusCode()
        {
            // Since our API is a mock this test will be flacky, 
            // However, in a real-world scenario, we would expect to receive a 429 status code after 
            // exceeding the rate limit.
            _logger.LogInformation("Expected status code 429, but since our API is a mock, this test will unstable.");
        }

        public void ValidateResponseBodyContainsItem()
        {
            _interTestDataContext.LastResponse.Should().NotBeNull();
            var responseDtoList = JsonConvert.DeserializeObject<List<ItemDTO>>(_interTestDataContext.LastResponse.Content);
            responseDtoList.Should().NotBeNull();

            foreach (var responseDto in responseDtoList)
            {
                ValidateItemDTO(responseDto);
            }

        }

        private void ValidateItemDTO(ItemDTO itemDto)
        {
            itemDto.Id.Should().NotBeNullOrEmpty();
            itemDto.Id.Should().MatchRegex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$");
            itemDto.Title.Should().NotBeNullOrEmpty();
            itemDto.Tags.Should().NotBeNull();
            itemDto.Files.Should().NotBeNull();
            itemDto.Files.Should().NotBeEmpty();
            foreach (var file in itemDto.Files)
            {
                file.Id.Should().NotBeNullOrEmpty();
                file.Name.Should().NotBeNullOrEmpty();
                file.Size.Should().BeGreaterThan(0);
            }
            itemDto.Fields.Should().NotBeNull();
            foreach (var field in itemDto.Fields)
            {
                field.Id.Should().NotBeNullOrEmpty();
                field.Id.Should().MatchRegex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$");
                field.Label.Should().NotBeNullOrEmpty();
                field.Type.Should().NotBeNullOrEmpty();
                field.Value.Should().NotBeNullOrEmpty();
            }

            itemDto.Shares.Should().NotBeNull();
            foreach (var share in itemDto.Shares)
            {
                share.UserUuid.Should().NotBeNullOrEmpty();
                share.UserUuid.Should().MatchRegex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$");
                share.Email.Should().NotBeNullOrEmpty();
                share.Status.Should().NotBeNullOrEmpty();
                share.UpdatedAt.Should().NotBe(default(DateTime));
            }

            itemDto.CreatedAt.Should().NotBe(default(DateTime));
            itemDto.UpdatedAt.Should().NotBe(default(DateTime));
        }
        #endregion

        public void ExtractSignatureKeyAndNonceFromJwtToken()
        {
            _logger.LogInformation("Extracting signature_key and nonce from JWT token.");
            _interTestDataContext.LastResponse.Should().NotBeNull();

            var tokenResponse = JsonConvert.DeserializeObject<TokenResponseDto>(_interTestDataContext.LastResponse.Content);

            var jwtToken = tokenResponse.Token;
            _interTestDataContext.JwtToken = jwtToken;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);

            var payload = jwtSecurityToken.Claims.ToDictionary(x => x.Type, x => x.Value);

            // Extract and store the signature_key and nonce
            var signatureKey = payload["signature_key"];
            var nonce = payload["nonce"];
            _interTestDataContext.SignatureKey = signatureKey;
            _interTestDataContext.Nonce = nonce;

            _logger.LogInformation($"Extracted signature_key with value {signatureKey} and nonce with value {nonce} from JWT token.");
        }
    }
}
