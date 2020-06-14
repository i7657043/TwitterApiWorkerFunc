using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using TOFunction;

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
               .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
               .AddEnvironmentVariables()
               .Build();

            string storageAccountConnectionString = string.Empty;

            if (Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Production")
                storageAccountConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            else if (Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development")
                storageAccountConnectionString = config.GetValue<string>("Values:AzureWebJobsStorage");       

            else
                Environment.Exit(-1);

            builder.Services
                   .AddOptions<StorageCredentials>()
                   .Configure<IConfiguration>((settings, config) =>
                   {
                       settings.AzureWebJobsStorage = storageAccountConnectionString;
                   });
        }
    }
}