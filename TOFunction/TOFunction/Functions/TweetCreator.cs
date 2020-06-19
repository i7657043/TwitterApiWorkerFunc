using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using Azure.Storage.Queues;
using System;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;
using TOFunction.Services.DatabaseService;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi;

namespace TOFunction
{
    public class TweetCreator
    {
        private readonly string _storageAccountConString;
        private readonly IDatabaseService _databaseService;
        public TweetCreator(
            IOptions<StorageCredentials> storageOptions,
            IDatabaseService databaseService)
        {
            _storageAccountConString = storageOptions.Value.AzureWebJobsStorage;            
            _databaseService = databaseService;
        }

        //[FunctionName(nameof(TweetCreator))]
        //public async Task Run([TimerTrigger("0 0 2 0 0 0")]TimerInfo myTimer, ILogger log)
        //{
        //    await Execute();
        //}


        [FunctionName(nameof(TweetCreator) + "http")]
        public async Task<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {            
            return await Execute();
        }

        private async Task<string> Execute()
        {
            try
            {
                //First clear all Queues from the previos days
                await ClearQueues();

                QueueClient scheuduleTimeQueueClient;

                //Create Tweets to send from DB
                Console.WriteLine("Creating Tweets from DB...");

                List<Tweet> tweetsToSend = await _databaseService.CreateTweetsAsync();

                //3 intervals for now
                List<List<string>> allTweets = tweetsToSend.GetTweetsForSchedules();

                foreach (List<string> tweetsList in allTweets)
                {
                    ScheduleTime tweetGroupsTime = JsonConvert.DeserializeObject<Tweet>(tweetsList[0]).ScheduleTime;

                    switch (tweetGroupsTime)
                    {
                        case ScheduleTime.Morning:
                            scheuduleTimeQueueClient = new QueueClient(_storageAccountConString, AzureResourceNames.MorningWaitingTweets);
                            break;
                        case ScheduleTime.Midday:
                            scheuduleTimeQueueClient = new QueueClient(_storageAccountConString, AzureResourceNames.MiddayWaitingTweets);
                            break;
                        case ScheduleTime.Afternoon:
                            scheuduleTimeQueueClient = new QueueClient(_storageAccountConString, AzureResourceNames.AfternoonWaitingTweets);
                            break;

                        default:
                            return "Fail - Tweet Schedule Time not recognised";
                    }

                    if (!await scheuduleTimeQueueClient.ExistsAsync())
                    {
                        Console.WriteLine($"Queues required dont exist. Exiting");
                        return "Fail - Queues required dont exist";
                    }

                    foreach (string serialisedTweet in tweetsList)
                    {
                        Console.WriteLine($"Sending Tweet {JsonConvert.DeserializeObject(serialisedTweet)}...");

                        await scheuduleTimeQueueClient.SendMessageAsync(serialisedTweet);

                        Console.WriteLine($"{tweetGroupsTime.ToString()} Tweet Sent to {scheuduleTimeQueueClient.Name} Queue");
                    }
                }

                Console.WriteLine($"Success, All Tweets sent to Waiting Queues");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");

                throw;
            }

            return "Success";
        }                

        private async Task ClearQueues()
        {
            QueueClient scheuduleTimeQueueClient = new QueueClient(_storageAccountConString, AzureResourceNames.MorningWaitingTweets);
            await scheuduleTimeQueueClient.ClearMessagesAsync();
            scheuduleTimeQueueClient = new QueueClient(_storageAccountConString, AzureResourceNames.MiddayWaitingTweets);
            await scheuduleTimeQueueClient.ClearMessagesAsync();
            scheuduleTimeQueueClient = new QueueClient(_storageAccountConString, AzureResourceNames.AfternoonWaitingTweets);
            await scheuduleTimeQueueClient.ClearMessagesAsync();
            scheuduleTimeQueueClient = new QueueClient(_storageAccountConString, AzureResourceNames.UnsentTweets);
            await scheuduleTimeQueueClient.ClearMessagesAsync();
        }
    }
}
