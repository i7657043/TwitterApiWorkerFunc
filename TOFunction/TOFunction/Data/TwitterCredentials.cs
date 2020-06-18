using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using TOFunction;

// register assembly
[assembly: FunctionsStartup(typeof(Startup))]
namespace TOFunction
{
    public class TwitterCredentials
    {
        public string ApiKey { get; set; }
        public string ApiSecretKey { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }
}