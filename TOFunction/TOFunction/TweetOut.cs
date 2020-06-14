using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

        public TweetOut(IOptions<StorageCredentials> options)
        {
            _storageAccountConString = options.Value.AzureWebJobsStorage;
        }

        [FunctionName(nameof(TweetOut) + "http")]
        public async Task<string> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            StringBuilder msg = new StringBuilder($"C# HTTP trigger function executed on Env: {Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT")} at: {DateTime.Now}");

            string blobContainerName = "testcontainer";

            try
            {
                // Create a BlobServiceClient object which will be used to create a container client
                BlobServiceClient blobService = new BlobServiceClient(_storageAccountConString);

                BlobContainerClient blobContainer = blobService.GetBlobContainerClient(blobContainerName);

                // Create the container and return a container client object
                //BlobContainerClient containerClient = await blobService.CreateBlobContainerAsync("");
                
                if (blobContainer.Exists())
                {
                    msg.Append($"\n\n{blobContainerName} Exists!");
                }
                else
                {
                    msg.Append($"\n\n{blobContainerName} Doesn't Exist...");
                }

                //Console.WriteLine("Listing blobs...");
                // List all blobs in the container
                //await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
                //{
                //    Console.WriteLine("\t" + blobItem.Name);
                //}
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
