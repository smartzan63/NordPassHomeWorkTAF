# NordPassHomeWorkTAF
 
## How to install
    This project is a .NET 6.0 application, so you'll need to have .NET 6.0 SDK installed on your machine. 
    If you don't have it installed, you can download it from the official .NET download page https://dotnet.microsoft.com/download
 
## Steps to run the project:
    Clone the repository: First, you'll need to clone the repository to your local machine. You can do this by running
    the following command in your terminal:

    Using Environment Variables for Credentials
    
    This project supports using environment variables for credentials. This can be useful if you want to avoid storing sensitive 
    information like usernames and passwords in your `appsettings.json` file.

    To use environment variables for credentials, you need to set 
    them in the following format: `SomeConfigSection__CredentialsClassName__FieldName`. 

    For example, if you have a configuration section named `Credentials`, a class named `TestApiCredentials`, and fields 
    named `Username` and `Password`, you would set the environment variables like this:
    
    - `Credentials__TestApiCredentials__Username`: This should be set to your username.
    - `Credentials__TestApiCredentials__Password`: This should be set to your password.
    
    These environment variables are read when the application starts, and they are used to populate the corresponding fields 
    in your configuration classes.

    Please note that the double underscore (`__`) is used as a separator in the environment variable names. This is a convention 
    used by the .NET configuration system to support hierarchical configuration data.

    Remember to replace `SomeConfigSection`, `CredentialsClassName`, `FieldName` with the actual names used in your configuration classes.

## Running the project with the console:
    1. Navigate to the project directory: Use the cd command to navigate to the directory where you cloned the repository. 
    For example:
    cd NordPassHomeWorkTAF

    2. Restore the NuGet packages: This project uses several NuGet packages, such as SpecFlow, FluentAssertions, etc. 
    You can restore these packages by running the following command:
    dotnet restore

    3. Build the project: To build the project, run the following command:
    dotnet build
    
    4. Run the tests: Finally, you can run the tests by executing the following command:
    dotnet test

## Running the project in Visual Studio 2022:
    1. Install Visual Studio 2022: If you don't have Visual Studio 2022 installed, you can download it from the official Visual Studio 
    download page. The Community version is free and has all the features needed for this project.
    
    2. Install SpecFlow extension: This project uses SpecFlow for behavior-driven development (BDD). To get the best experience with 
    SpecFlow in Visual Studio, it's recommended to install the SpecFlow extension. You can do this by going to 
    Extensions > Manage Extensions in Visual Studio, and then search for "SpecFlow". Click on "SpecFlow for Visual Studio 2022" in the 
    search results and then click on the "Download" button. You'll need to restart Visual Studio to complete the installation. 
    
    3. Open the project: Open Visual Studio and click on File > Open > Project/Solution. Navigate to the directory where you cloned 
    the repository and select the .sln file.

    4.Restore the NuGet packages: Right-click on the solution in the Solution Explorer and select Restore NuGet Packages.

    5.Build the project: You can build the project by clicking on Build > Build Solution.
    6.Run the tests: To run the tests, you can open the Test Explorer by clicking on Test > Test Explorer. In the Test Explorer,
    you can run all tests by clicking on the "Run All Tests" button, or you can run individual tests or groups of tests by right-clicking
    on them and selecting "Run".

## Notes:
    This project uses SpecFlow for behavior-driven development (BDD). The test scenarios are defined in .feature files, and the 
    step definitions are in corresponding .cs files.
    The project also uses Playwright for UI automation. Make sure you have the necessary browsers installed on your machine.
    The project uses FluentAssertions for assertions, and System.IdentityModel.Tokens.Jwt for handling JWT tokens.
    The configuration for the tests (like base URLs, credentials, etc.) is stored in the appsettings.json file. You can modify this 
    file to change the configuration for your tests.
    The project uses NUnit as the test framework. The test results are displayed in the NUnit format.
    The project uses the Microsoft.Extensions.Logging library for logging. The logs are displayed in the console and can be configured to 
    be written to other outputs as well.

##NordPass homework (automation task)
    Open question:
        Do you see any security risks with given User API response objects? If so, please
        identify them.

    Answers:
        1. Excessive Information and File Path Exposure: The API responses, such as the response to the GET /user/items request, 
        contain a lot of information that might not be necessary for the client.This could lead to data leakage if these responses 
        are intercepted or stolen. For instance, the response to the GET /user/{id}/item request returns file information, including its path.
        This could pose a potential security threat as an attacker could use this information to gain access to the files. 
        It would be safer to use a system of references or aliases that map to the actual file paths on the server side, rather than 
        exposing the real paths.

        2. Password Fields in API Response: The response to the GET /user/{id}/item and POST /user/item requests returns a password field.
        This is a serious security breach as passwords should not be transmitted or stored in plain text. 
        Instead, the system should use password hashing for secure storage and verification.
        
        3. Sensitive Data Exposure: The API responses contain sensitive data, such as user UUIDs and item IDs. 
        These identifiers could potentially be used for malicious purposes if they fall into the wrong hands. 
        It would be advisable to consider an additional layer of security, such as data obfuscation or encryption, 
        to further protect this information.

        4. JWT Token Exposure: The API response for the login endpoint includes a JWT token. 
        While JWTs are a common method for handling user sessions, they can pose a security risk if not handled properly. 
        It's crucial to ensure that these tokens are stored securely on the client side and not exposed to potential XSS attacks. 
        Additionally, the JWT should be signed and verified on the server side to prevent tampering.

    Bonus (optional) 
        Please share any non-functional tests you can think of related to the first or second challenge.
        
        For the first challenge, which is UI automation, non-functional tests could include:   
        
            1. Performance Testing: This would involve testing the load time of the NordPass homepage and other pages that the user 
            navigates to. This is important as users may abandon a website if it takes too long to load.
                        
            2. Usability Testing: This would involve testing the user interface and overall user experience of the NordPass website. 
            This could include checking the intuitiveness of the navigation, the readability of the text, the visibility of important 
            elements, etc.

            3. Accessibility Testing: This would involve testing the website's accessibility features, such as screen reader compatibility, 
            keyboard navigation, color contrast, etc. This is important to ensure that the website is usable by people with 
            various disabilities.
    
            4. Security Testing: This would involve testing the security features of the website, such as SSL/TLS encryption, secure 
            cookies, Content Security Policy, etc. This is important to protect users' sensitive data.

            5. Compatibility Testing: This would involve testing the website on various devices, browsers, and screen sizes to 
            ensure that it works correctly and looks good on all platforms.

        For the second challenge, which is API testing, non-functional tests could include:
        
            1. Performance Testing: This would involve testing the response time of the API endpoints under different loads. 
            This is important to ensure that the API can handle a large number of requests without slowing down.
            
            2. Security Testing: This would involve testing the security features of the API, such as authentication, authorization, 
            data encryption, etc. This is important to protect users' sensitive data.
            
            3. Reliability Testing: This would involve testing the API's error handling and recovery mechanisms. This is 
            important to ensure that the API can recover from failures and continue to function correctly.
            
            4. Compatibility Testing: This would involve testing the API with different versions of the client software to ensure
            that it is backward compatible.    
            
            5. Scalability Testing: This would involve testing the API's ability to handle an increasing amount of work 
            by adding resources to the system. 
            This is important to ensure that the API can scale to meet demand.