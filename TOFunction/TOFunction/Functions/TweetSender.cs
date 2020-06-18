using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace TOFunction
{

    public class TweetSender
    {
        //[FunctionName(nameof(TweetOut))]
        //public static void Run([TimerTrigger("* * * * * *")]TimerInfo myTimer, ILogger log)
        //{
        //    log.LogInformation($"\n\n\nC# Timer trigger function executed at: {DateTime.Now}\n\n\n");
        //}

        private readonly string _storageAccountConString;

        public TweetSender(IOptions<StorageCredentials> options)
        {
            _storageAccountConString = options.Value.AzureWebJobsStorage;
        }

        [FunctionName(nameof(TweetSender))]
        public static void Run([QueueTrigger("unsent-tweets", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
        {
            Console.WriteLine($"Retrieved message with content '{myQueueItem}'. Deserialising Message...");

            UnsentTweet test = JsonConvert.DeserializeObject<UnsentTweet>(myQueueItem);

            //Tweet out message

            // TODO: Need to delete msg if using Queue Trigger?
        }        
    }
}
