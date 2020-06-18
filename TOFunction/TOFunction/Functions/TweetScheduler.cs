using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TOFunction.Functions
{
    public class TweetScheduler
    {
        private readonly string _storageAccountConString;

        public TweetScheduler(IOptions<StorageCredentials> storageOptions)
        {
            _storageAccountConString = storageOptions.Value.AzureWebJobsStorage;
        }

        //At 10:00,10:15,10:30,10:45,AM, 1:00,1:15,1:30,1:45PM and 3:00,3:15,3:30,3:45PM every weekday Max
        [FunctionName(nameof(TweetScheduler))]
        public async Task<string> Run([TimerTrigger("0 0,15,30,45 10,13,15 * * 1-5")]TimerInfo myTimer, ILogger log) 
        {
            switch(DateTime.Now.TimeOfDay.Hours)
            {
                case 10:
                    return await ScheduleNextTweet(new QueueClient(_storageAccountConString, QueueNames.MorningWaitingTweets));

                case 13:
                    return await ScheduleNextTweet(new QueueClient(_storageAccountConString, QueueNames.MiddayWaitingTweets));

                case 15:
                    return await ScheduleNextTweet(new QueueClient(_storageAccountConString, QueueNames.AfternoonWaitingTweets));

                default:
                    return "Fail - Incorrect time execution";
            }
        }

        private async Task<string> ScheduleNextTweet(QueueClient queueClient)
        {
            QueueMessage[] retrievedMessage = await queueClient.ReceiveMessagesAsync();

            string tweetToSchedule = retrievedMessage[0].MessageText.DecodeBase64();

            QueueClient unsentTweetsQueueClient = new QueueClient(_storageAccountConString, QueueNames.UnsentTweets);

            await unsentTweetsQueueClient.SendMessageAsync(tweetToSchedule);

            return "Success";
        }

        [FunctionName(nameof(TweetScheduler) + "http")]
        public async Task<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            return await ScheduleNextTweet(new QueueClient(_storageAccountConString, QueueNames.MorningWaitingTweets));
        }
    }
}
