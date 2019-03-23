using System;
using System.Threading.Tasks;
using ImagesWorkerAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace ImagesWorkerAPI.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        private readonly CloudStorageAccount account;
        private readonly CloudQueueClient queueClient;

        public QueueService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("StorageAccountConnectionString");

            this.configuration = configuration ?? throw new ArgumentNullException($"Configuration {nameof(configuration)} has null value");

            connectionString = configuration.GetConnectionString("StorageAccountConnectionString");

            account = CloudStorageAccount.Parse(this.connectionString);

            queueClient = account.CreateCloudQueueClient();
        }

        public async Task AddMessageAsync(string message)
        {
            CloudQueue queue = queueClient.GetQueueReference("imagemessagequeue");

            await queue.CreateIfNotExistsAsync();

            CloudQueueMessage cloudMessage = new CloudQueueMessage(message);

            await queue.AddMessageAsync(cloudMessage);
        }
    }
}