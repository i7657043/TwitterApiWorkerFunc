using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using TOFunction.Data;
using TOFunction.Extensions;

namespace TOFunction.Functions
{
    public class TweetScheduler
    {
        private readonly string _storageAccountConString;

        public TweetScheduler(IOptions<AzureStorageCredentials> storageOptions)
        {
            _storageAccountConString = storageOptions.Value.AzureWebJobsStorage;
        }

        //At 10:00,10:15,10:30,10:45,AM, 1:00,1:15,1:30,1:45PM and 3:00,3:15,3:30,3:45PM every weekday Max
        //[FunctionName(nameof(TweetScheduler))]
        //public async Task Run([TimerTrigger("0 0,15,30,45 10,13,15 * * 1-5")]TimerInfo myTimer, ILogger log)
        //{
        //    await Execute();
        //}

        [FunctionName(nameof(TweetScheduler) + "http")]
        public async Task<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            return await Execute();
        }

        private async Task<string> Execute()
        {
            try
            {
                switch (DateTime.Now.TimeOfDay.Hours)
                {
                    case 10:
                        return await ScheduleNextTweetOnQueue(new QueueClient(_storageAccountConString, AzureResourceNames.MorningWaitingTweets));
                    case 13:
                        return await ScheduleNextTweetOnQueue(new QueueClient(_storageAccountConString, AzureResourceNames.MiddayWaitingTweets));
                    case 22:
                        return await ScheduleNextTweetOnQueue(new QueueClient(_storageAccountConString, AzureResourceNames.AfternoonWaitingTweets));

                    default:
                        return $"Fail - Incorrect timer execution {DateTime.Now}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");

                throw;
            }
            
        }

        private async Task<string> ScheduleNextTweetOnQueue(QueueClient queueClient)
        {
            QueueMessage[] retrievedMessage = await queueClient.ReceiveMessagesAsync(1);

            QueueClient unsentTweetsQueueClient = new QueueClient(_storageAccountConString, AzureResourceNames.UnsentTweets);

            await unsentTweetsQueueClient.SendMessageAsync(retrievedMessage[0].MessageText.EncodeBase64());

            return "Success";
        }        
    }
}
