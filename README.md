# public-forms

Please note that this is just a sample application, delivered under attached license.


Application creates HTML forms from BPS forms configuration.
 
#### Installation on IIS
1. Configure IIS and application pool (the user running the application in IIS must have write permission).
2. Fill in the AppSettings file (for the appropriate environment - AppSettings.Production.json, for development AppSettings.Development.json) - Login and Password.
3. After starting the application and logging in, fill in the data on the configuration page.
4. ClientId set in configuration has to have rights to application and start elements for this application.

#### Installation on Azure
1. Run the scripts available in the project in the Azure Scripts directory (AppService and KeyVault).
2. After adding AppService, you must enable Identity section for it in Azure.
3. Fill in parameter values in Azure Key Vault.
4. Before publishing to Azure, please make sure that the KeyVaultEndpoint field value in Program.cs matches the Key Vault you created on Azure.
5. After starting the application and logging in, fill in the data on the configuration page. 
6. ClientId set in configuration has to have rights to application and start elements for this application.

#### Solution projects:
 1. Infrastructure:
	* WEBCON.FormsGenerator.API - connects with BPS API. Currently use beta API.
	* WEBCON.FormsGenerator.Data - project with database support (EF).
2. Test:
	* WEBCON.FormsGenerator.Test - application tests
3. Application
	* WEBCON.FormsGenerator.BusinessLogic - project with application business logic. Bridge between infrastructure
	and presentation layer. Contains CRUD support on domain objects, generating forms body etc.
	* WEBCON.FormsGenerator.Presentation - project with application presentation layer.
 
 
 If enviromental appsettings are not in gitignore use these commands after Git Clone:
 
 * git update-index --skip-worktree appsettings.Development.json
 * git update-index --skip-worktree appsettings.Staging.json
 * git update-index --skip-worktree appsettings.Production.json