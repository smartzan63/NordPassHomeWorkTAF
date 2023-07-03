using NordPassHomeWorkTAF.Contexts;

namespace NordPassHomeWorkTAF.Steps
{
    [Binding]
    public class APISteps
    {
        private readonly HomeWorkApiContext _apiContext;

        public APISteps(HomeWorkApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        [StepDefinition(@"I send API POST request to '([^']*)' endpoint with valid credentials")]
        public async Task SendAPIPOSTRequestToLoginEndpointWithValidCredentials(string endpointName)
        {
            await _apiContext.SendAPIPOSTRequestToLoginEndpointWithValidCredentialsAsync(endpointName);
        }

        [StepDefinition(@"I send API POST request to '(.*)' endpoint with valid item data by file key '(.*)'")]
        public async Task SendAPIPOSTRequestWithValidItemData(string endpointName, string fileKey)
        {
            await _apiContext.SendAPIPOSTRequestWithValidItemDataAsync(endpointName, fileKey);
        }

        [StepDefinition(@"I send API request using endpoint '([^']*)'")]
        public async Task SendAPIRequestUsingEndpoint(string endpointName)
        {
            await _apiContext.SendAPIRequestUsingEndpointAsync(endpointName);
        }

        [StepDefinition(@"I send API request to get item at index '(\d+)'")]
        public async Task SendAPIRequestToGetItemAtIndex(int index)
        {
            await _apiContext.SendAPIRequestToGetItemAtIndexAsync(index);
        }


        [StepDefinition(@"The response code is ""([^""]*)""")]
        public void ThenTheResonseCodeIs(string httpStatusCode)
        {
            _apiContext.ValidateStatusCode(httpStatusCode);
        }

        [StepDefinition(@"Response body condtains a list of items UUID")]
        public void ThenResponseBodyCondtainsAListOfItemsUUID()
        {
            _apiContext.ValidateResponseBodyContainsListOfItemsUUID();
        }

        [StepDefinition(@"Response body condtains an item")]
        public void ThenResponseBodyCondtainsAnItem()
        {
            _apiContext.ValidateResponseBodyContainsItem();
        }

        [StepDefinition(@"I validate JWT token from response")]
        public void ThenIValidateJWTTokenFromResponse()
        {
            _apiContext.ValidateJwtTokenFromResponse();
        }

        [StepDefinition(@"Response body contains the created item")]
        public void ThenResponseBodyContainsTheCreatedItem()
        {
            _apiContext.ValidateResponseBodyWithExpectedItem("NordPassHomeWorkTAF.Resources.DefaultItemRequestBody.json");
        }

        [StepDefinition(@"I wait up to '([^']*)' seconds for the created item to be available by '([^']*)' endpoint")]
        public async Task WhenIWaitUpToSecondsForTheCreatedItemToBeAvailableByEndpoint(string timeOutSeconds, string endpointName)
        {
            await _apiContext.WaitForCreatedItemToAppearInUserItemsEndpointAsync(endpointName, timeOutSeconds);
        }

        [StepDefinition(@"I store the '([^']*)' value from the response headers")]
        public void ThenIStoreTheValueFromTheResponseHeaders(string headerName)
        {
            _apiContext.StoreRateLimitFromResponseHeaders(headerName);
        }

        [StepDefinition(@"I send API POST request to '([^']*)' endpoint with valid credentials more than x-rate-limit times")]
        public async Task WhenISendAPIPOSTRequestToEndpointWithValidCredentialsForX_Rate_LimitTimes(string endpointName)
        {
            await _apiContext.SendLoginRequestMultipleTimesAsync(endpointName);
        }

        [StepDefinition(@"I wait for login limit reset and login again by '([^']*)' endpoint")]
        public async Task ThenIWaitForLoginLimitResetAndLoginAgain(string endpointName)
        {
            await _apiContext.WaitForLoginLimitResetAndLoginAgainAsync(endpointName);
        }

        [StepDefinition(@"The response code is Too Many Requests")]
        public void ThenTheResponseCodeIsTooManyRequests()
        {
            _apiContext.ValidateTooManyRequestsStatusCode();
        }

        [StepDefinition(@"I extract the 'signature_key' and 'nonce' from the JWT token")]
        public void ExtractSignatureKeyAndNonceFromJwtToken()
        {
            _apiContext.ExtractSignatureKeyAndNonceFromJwtToken();
        }

        [StepDefinition(@"I send request to GET user items with generated HMAC headers")]
        public async Task SendRequestToGetUserItemsWithGeneratedHeaders()
        {
            await _apiContext.SendGetUserItemsRequestWithHmacHeaders("UserItems");
        }
    }
}
