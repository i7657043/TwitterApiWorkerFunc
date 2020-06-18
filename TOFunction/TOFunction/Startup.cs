using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using TOFunction;
using TOFunction.Services.DatabaseService;

// register assembly
[assembly: FunctionsStartup(typeof(Startup))]
namespace TOFunction
{
    // inherit FunctionsStartup
    public class Startup : FunctionsStartup
    {
        // override configure method
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddUserSecrets(Assembly.GetExecutingAssembly(), false, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

            string storageAccountConnectionString = string.Empty;
            string dbConnectionString = string.Empty;
            string twitterApiKey = string.Empty;
            string twitterApiSecretKey = string.Empty;
            string twitterAccessToken = string.Empty;
            string twitterAccessTokenSecret = string.Empty;

            if (Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Production")
                storageAccountConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            else if (Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development")
                storageAccountConnectionString = config.GetValue<string>("Values:AzureWebJobsStorage");       
            
            dbConnectionString = config.GetConnectionString("Rdb");

            twitterApiKey = config.GetValue<string>("TwitterCredentials:ApiKey");
            twitterApiSecretKey = config.GetValue<string>("TwitterCredentials:ApiSecretKey");
            twitterAccessToken = config.GetValue<string>("TwitterCredentials:AccessToken");
            twitterAccessTokenSecret = config.GetValue<string>("TwitterCredentials:AccessTokenSecret");

            if (storageAccountConnectionString == string.Empty || dbConnectionString == string.Empty)
            {
                Console.WriteLine("\nNo Valid Connection to Storage or Database. Exiting");
                return;
            }

            builder.Services
                   .AddOptions<StorageCredentials>()
                   .Configure<IConfiguration>((settings, config) =>
                   {
                       settings.AzureWebJobsStorage = storageAccountConnectionString;
                   });

            builder.Services
                   .AddOptions<DatabaseCredentials>()
                   .Configure<IConfiguration>((settings, config) =>
                   {
                       settings.DbConnectionString = dbConnectionString;
                   });

            builder.Services
                   .AddOptions<TwitterCredentials>()
                   .Configure<IConfiguration>((settings, config) =>
                   {
                       settings.ApiKey = twitterApiKey;
                       settings.ApiSecretKey = twitterApiSecretKey;
                       settings.AccessToken = twitterAccessToken;
                       settings.AccessTokenSecret = twitterAccessTokenSecret;                       
                   });

            builder.Services
                .AddSingleton<IDatabaseService, DatabaseService>()
                .AddSingleton<IDatabaseProvider, DatabaseProvider>()
                .AddSingleton<IDatabaseRepository, DatabaseRepository>();
        }
    }
}