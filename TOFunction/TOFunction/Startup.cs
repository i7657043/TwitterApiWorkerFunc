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
               .AddJsonFile("appsettings.json", true)
               .AddEnvironmentVariables()
               .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
               .Build();

            string storageAccountConnectionString = string.Empty;
            string dbConnectionString = string.Empty;

            if (Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Production")
                storageAccountConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            else if (Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development")
                storageAccountConnectionString = config.GetValue<string>("Values:AzureWebJobsStorage");       
            
            dbConnectionString = config.GetConnectionString("Rdb");

            if (storageAccountConnectionString == string.Empty || dbConnectionString == string.Empty)
            {
                Console.WriteLine("\nNo Valid Connection to Storage or Database. Exiting");
                Environment.Exit(-1);
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
                .AddSingleton<IDatabaseService, DatabaseService>()
                .AddSingleton<IDatabaseProvider, DatabaseProvider>()
                .AddSingleton<IDatabaseRepository, DatabaseRepository>();
        }
    }
}