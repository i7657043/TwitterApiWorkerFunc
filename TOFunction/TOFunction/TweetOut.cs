using System;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;

using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage

namespace TOFunction
{
    public class TweetOut
    {
        //[FunctionName(nameof(TweetOut))]
        //public static void Run([TimerTrigger("* * * * * *")]TimerInfo myTimer, ILogger log)
        //{
        //    log.LogInformation($"\n\n\nC# Timer trigger function executed at: {DateTime.Now}\n\n\n");
        //}

        private readonly string _storageAccountConString;
        private readonly string blobContainerName = "testcontainer";
        private readonly CloudStorageAccount _cloudStorageAccount;

        public TweetOut(IOptions<StorageCredentials> options)
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(options.Value.AzureWebJobsStorage);
        }

        [FunctionName(nameof(TweetOut) + "http")]
        public async Task<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            StringBuilder msg = new StringBuilder($"C# HTTP trigger function executed on Env: {Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT")} at: {DateTime.Now}");
            
            try
            {
                // Create a BlobServiceClient object which will be used to create a container client
                BlobServiceClient blobService = new BlobServiceClient(_storageAccountConString);

                BlobContainerClient blobContainer = blobService.GetBlobContainerClient(blobContainerName);
                
                if (!blobContainer.Exists())
                {
                    msg.Append($"Creating new Blob Container: \"{blobContainerName}\"");

                    Response<BlobContainerClient> response = await blobService.CreateBlobContainerAsync(blobContainerName);
                }

                Console.WriteLine("Listing blobs...");

                await foreach (BlobItem blobItem in blobContainer.GetBlobsAsync())
                {                                        
                    Console.WriteLine("\t" + blobItem.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");

                throw;
            }

            log.LogInformation(msg.ToString());

            return msg.ToString();
        }
    }
}
