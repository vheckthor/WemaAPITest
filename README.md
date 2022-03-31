# WemaAPITest
Build a Microservice Api that would have the following endpoints:
 
1. Onboard a customer: The endpoint should take phone Number, email , password, state of residence, and LGA.
 
Business Rule
 
(i) The phone number should be verified via OTP before an onboarding is said to be completed
 
(ii) The lga must be mapped to the state selected.
 
(iii) Mock sending the OTP.
 
 
2. Endpoint to return all existing customers previously onboarded
 
 
3. An endpoint that will return existing Bank: browse the link below, you will find an endpoint called Getbanks. consume that endpoint and output the result you  
 
https://wema-alatdev-apimgt.developer.azure-api.net/api-details#api=alat-tech-test-api
 
 
Note.
 
 - Use Asp dotnet core to build the api, add swagger documentation
 - Use Microsoft sql db and entity framework.
 - ensure that you apply all standard engineering principle, be mindful of the pattern you apply and also consider adding unit testing
 
- push your code to a public git repo and share the link

## Running the project
 - Pull the code and from dotnet cli or package manager console run `dotnet restore` to install the required packages.
 - Add the desired connection string or leave the default (provided Microsoft Sql is installed on the machine.
 - For the `GetAllBanks` endpoint you will require a subscription key to be added as your environment variable as `subscriptionKey`, you can get you subscription key by registering and login here https://wema-alatdev-apimgt.developer.azure-api.net/product#product=alat-tesh-test.
 - run `[System.Environment]::SetEnvironmentVariable('subscriptionKey','your key here',[System.EnvironmentVariableTarget]::Machine)` to set the environment variable
 - Build the project from the cli or visual studio and launch the url to see the swagger Api documentation.
