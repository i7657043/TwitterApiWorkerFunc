using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using TOFunction;

// register assembly
[assembly: FunctionsStartup(typeof(Startup))]
namespace TOFunction
{
    public class StorageCredentials
    {
        public string AzureWebJobsStorage { get; set; }
    }
}