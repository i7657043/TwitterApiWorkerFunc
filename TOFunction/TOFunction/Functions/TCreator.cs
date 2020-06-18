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

namespace TOFunction
{
    public class TCreator
    {
        //[FunctionName(nameof(TweetOut))]
        //public static void Run([TimerTrigger("* * * * * *")]TimerInfo myTimer, ILogger log)
        //{
        //    log.LogInformation($"\n\n\nC# Timer trigger function executed at: {DateTime.Now}\n\n\n");
        //}

        private readonly IDatabaseService _databaseService;
        private readonly string _storageAccountConString;

        public TCreator(IOptions<StorageCredentials> storageOptions, IDatabaseService databaseService)
        {
            _storageAccountConString = storageOptions.Value.AzureWebJobsStorage;
            _databaseService = databaseService;
        }

        [FunctionName(nameof(TCreator) + "http")]
        public async Task<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            StringBuilder msg = new StringBuilder($"C# HTTP trigger function executed on Env: {Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT")} at: {DateTime.Now}");

            try
            {
                // Instantiate a QueueClient for each Queue
                QueueClient unsentTweetsQueueClient = new QueueClient(_storageAccountConString, QueueNames.UnsentTweets);

                QueueClient scheuduleTimeQueueClient;

                if (!await unsentTweetsQueueClient.ExistsAsync())
                {
                    Console.WriteLine($"Queues required dont exist. Exiting");
                    return "Fail - Queues required dont exist";
                }

                //Create Tweets to send from DB
                Console.WriteLine("Creating Tweets from DB...");
                List<UnsentTweet> tweetsToSend = await _databaseService.CreateTweetsAsync();

                //3 intervals for now
                List<List<string>> allTweets = new List<List<string>>();
                allTweets.Add(tweetsToSend.GetTweetsForTimeOfDay(ScheduleTime.Morning));
                allTweets.Add(tweetsToSend.GetTweetsForTimeOfDay(ScheduleTime.Midday));
                allTweets.Add(tweetsToSend.GetTweetsForTimeOfDay(ScheduleTime.Afternoon));

                foreach (List<string> tweetsList in allTweets)
                {
                    ScheduleTime tweetGroupsTime = JsonConvert.DeserializeObject<UnsentTweet>(tweetsList[0]).ScheduleTime;

                    switch (tweetGroupsTime)
                    {
                        case ScheduleTime.Morning:
                            scheuduleTimeQueueClient = new QueueClient(_storageAccountConString, QueueNames.MorningWaitingTweets);
                            break;
                        case ScheduleTime.Midday:
                            scheuduleTimeQueueClient = new QueueClient(_storageAccountConString, QueueNames.MiddayWaitingTweets);
                            break;
                        case ScheduleTime.Afternoon:
                            scheuduleTimeQueueClient = new QueueClient(_storageAccountConString, QueueNames.AfternoonWaitingTweets);
                            break;

                        default:
                            return "Fail - Tweet Schedule Time not recognised";
                    }

                    foreach (string serialisedTweet in tweetsList)
                    {
                        Console.WriteLine($"Sending Tweet {JsonConvert.DeserializeObject(serialisedTweet)}...");

                        await scheuduleTimeQueueClient.SendMessageAsync(serialisedTweet);

                        Console.WriteLine($"{tweetGroupsTime.ToString()} Tweet Sent");
                    }
                }                

                Console.WriteLine($"Success, Tweet sending Complete");

                // Async receive the message
                // QueueMessage[] retrievedMessage = await queueClient.ReceiveMessagesAsync();
                // string decodedMsg = retrievedMessage[0].MessageText.DecodeBase64();

                //Console.WriteLine($"Retrieved message with content '{decodedMsg}'");
                //Console.WriteLine("Deserialising Message...");
                //UnsentTweet test = JsonConvert.DeserializeObject<UnsentTweet>(decodedMsg);

                // Async delete the message
                //await queueClient.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                //Console.WriteLine($"Deleted message: '{retrievedMessage[0].MessageText}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");

                throw;
            }

            log.LogInformation(msg.ToString());

            return "Success";
        }
    }
}
