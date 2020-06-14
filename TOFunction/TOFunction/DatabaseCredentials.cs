using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using TOFunction;

// register assembly
[assembly: FunctionsStartup(typeof(Startup))]
namespace TOFunction
{
    public class DatabaseCredentials
    {
        public string DbConnectionString { get; set; }
    }
}