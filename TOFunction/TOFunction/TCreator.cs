using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TOFunction
{
    public class TCreator
    {
        //[FunctionName(nameof(TweetOut))]
        //public static void Run([TimerTrigger("* * * * * *")]TimerInfo myTimer, ILogger log)
        //{
        //    log.LogInformation($"\n\n\nC# Timer trigger function executed at: {DateTime.Now}\n\n\n");
        //}

        //private readonly string _dbConString;
        //private readonly string blobContainerName = "testcontainer";

        //public TCreator(IOptions<DatabaseCredentials> options)
        //{
        //    _dbConString = options.Value.DbConnectionString;
        //}

        //[FunctionName(nameof(TCreator) + "http")]
        //public async Task<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        //{

        //}
    }
}
