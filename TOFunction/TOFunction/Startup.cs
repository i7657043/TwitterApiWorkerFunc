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
            
            string storageAccountConnectionString = config.GetValue<string>("Values:AzureWebJobsStorage");

            builder.Services
                .AddOptions<StorageCredentials>()
                .Configure<IConfiguration>((settings, config) =>
                {
                    settings.AzureWebJobsStorage = storageAccountConnectionString;
                });
        }
    }
}