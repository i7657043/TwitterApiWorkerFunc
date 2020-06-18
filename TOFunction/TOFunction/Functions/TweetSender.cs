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
        [FunctionName(nameof(TweetSender))]
        public static void Run([QueueTrigger("unsent-tweets")] string myQueueItem, ILogger log)
        {
            Console.WriteLine($"Retrieved message with content '{myQueueItem}'. Deserialising Message...");

            UnsentTweet test = JsonConvert.DeserializeObject<UnsentTweet>(myQueueItem);

            //Tweet out message
        }
    }
}
