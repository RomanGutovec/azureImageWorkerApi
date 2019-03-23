using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImagesWorkerAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ImagesWorkerAPI.Repositories
{
    public class BlobService : IBlobService
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        private readonly CloudStorageAccount account;
        private readonly CloudBlobClient blobClient;

        public BlobService(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException($"Configuration {nameof(configuration)} has null value");

            connectionString = configuration.GetConnectionString("StorageAccountConnectionString");
            account = CloudStorageAccount.Parse(this.connectionString);
            blobClient = account.CreateCloudBlobClient();
        }

        public async Task UploadBlobAsync(Stream stream, string fileName)
        {
            BlobRequestOptions bro = new BlobRequestOptions()
            {
                SingleBlobUploadThresholdInBytes = 1024 * 1024,
                ParallelOperationThreadCount = 3
            };

            blobClient.DefaultRequestOptions = bro;

            CloudBlobContainer blobContainer = blobClient.GetContainerReference("blobcontainer");
            await blobContainer.CreateIfNotExistsAsync();

            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);
            await blob.UploadFromStreamAsync(stream);
        }

        public async Task<byte[]> GetBlobByUrlAsync(string nameImage)
        {
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("blobcontainer");
            await blobContainer.CreateIfNotExistsAsync();
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(nameImage);

            await blockBlob.FetchAttributesAsync();

            byte[] arr = new byte[blockBlob.Properties.Length];
            await blockBlob.DownloadToByteArrayAsync(arr, 0);

            return arr;
        }

        private async Task<CloudBlobContainer> CreateContainerIfNotExistAsync(string containerName)
        {
            var container = blobClient.GetContainerReference(containerName);
            if (!await container.ExistsAsync())
            {
                await container.CreateAsync();
                await container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }

            return container;
        }
    }
}
