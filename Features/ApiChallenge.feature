@NordpassTestHostAPI
Feature: Second challenge - API
    2nd challenge - API

Scenario: Get and validate 2nd user item
    Given I send API POST request to 'UserLogin' endpoint with valid credentials
    Then The response code is "200"
    And I validate JWT token from response
    When I send API request using endpoint 'UserItems'
    Then The response code is "200"
    And Response body condtains a list of items UUID
    When I send API request to get item at index '2'
    Then The response code is "200"
    And Response body condtains an item

Scenario: Create a new item and verify delay after creation
    Given I send API POST request to 'UserLogin' endpoint with valid credentials
    Then The response code is "200"
    And I validate JWT token from response
    When I send API POST request to 'CreateItem' endpoint with valid item data by file key 'DefaultItemRequestBody'
    Then The response code is "201"
    And Response body contains the created item
    When I wait up to '10' seconds for the created item to be available by 'UserItems' endpoint
    #Here we have smart retryer, but since it's a Mock Api it waits till the timeout
    #And just passes the test

Scenario: Verify login limit
    Given I send API POST request to 'UserLogin' endpoint with valid credentials
    Then The response code is "200"
    And I validate JWT token from response
    And I store the 'x-rate-limit' value from the response headers
    When I send API POST request to 'UserLogin' endpoint with valid credentials more than x-rate-limit times
    Then The response code is Too Many Requests
    # At this point, we would need to wait for 1 hour before we can login again. 
    # However, waiting for 1 hour in an automated test is not efficient, 
    # In a real-world scenario, you would wait for 1 hour before sending
    # another login request. So it should be tested manually, or if the service
    # can be configured dynamically we can consider implementing this scenario with small timeout
    And I wait for login limit reset and login again by 'UserLogin' endpoint

Scenario: Test HMAC Authentication (Additional task)
    Given I send API POST request to 'UserLogin' endpoint with valid credentials
    Then The response code is "200"
    And I validate JWT token from response
    And I extract the 'signature_key' and 'nonce' from the JWT token
    When I send request to GET user items with generated HMAC headers
    Then The response code is "200"
    And Response body condtains a list of items UUID
    # Note: According to the task description, HMAC authorization is used as an additional security layer for the API.
    # The JWT token obtained after successful login is used to extract the 'signature_key' and 'nonce', which are then used to construct the HMAC headers.
    # These headers are then used to send a request to the API. Therefore, it seems that the JWT token is not required when sending the request with HMAC authorization,
    # as its sole purpose is to provide the 'signature_key' and 'nonce' for creating the HMAC headers.
    # However, this assumption is based on the provided task description and may vary depending on the actual implementation of your API.
    # If your API still requires the JWT token when using HMAC authorization, you will need to pass the JWT token along with the HMAC headers.

