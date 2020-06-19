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
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using TOFunction.Services.DatabaseService;
using Tweetinvi;

namespace TOFunction
{

    public class TweetSender
    {
        private readonly TwitterCredentials _twitterCredentials;
        private readonly string _storageAccountConString;

        public TweetSender(IOptions<TwitterCredentials> twitterCredentials, IOptions<StorageCredentials> storageOptions)
        {
            _twitterCredentials = twitterCredentials.Value;
            _storageAccountConString = storageOptions.Value.AzureWebJobsStorage;
        }

        [FunctionName(nameof(TweetSender))]
        public async Task Run([QueueTrigger("unsent-tweets")] string queueMessage)
        {
            await Execute(queueMessage);
        }

        private async Task Execute(string queueMessage)
        {
            try
            {
                Console.WriteLine($"Retrieved message with content '{queueMessage}'. Deserialising Message...");

                Tweet tweet = JsonConvert.DeserializeObject<Tweet>(queueMessage);

                Auth.SetUserCredentials(_twitterCredentials.ApiKey, _twitterCredentials.ApiSecretKey, _twitterCredentials.AccessToken, _twitterCredentials.AccessTokenSecret);

                string tweetToPublish = tweet.GeneratePublishableTweet();

                //Tweet.PublishTweet(tweetToPublish);

                Console.WriteLine($"Tweet sent successfully");

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageAccountConString);

                await WriteTweetDetailsToTableStorage(tweet, storageAccount);

                Console.WriteLine($"Tweet details successfully wrote to Table Storage");

                Console.WriteLine($"Finsihed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");

                throw;
            }
        }

        private static async Task WriteTweetDetailsToTableStorage(Tweet tweet, CloudStorageAccount storageAccount)
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(AzureResourceNames.TweetLogsTable);

            TweetTableEntity tte = tweet.MapToTableEntity();

            TableOperation insert = TableOperation.Insert(tte);

            await table.ExecuteAsync(insert);
        }
    }
}
