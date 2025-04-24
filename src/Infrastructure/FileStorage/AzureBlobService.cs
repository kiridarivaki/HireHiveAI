using Application.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace HireHive.Infrastructure.FileStorage
{
    public class AzureBlobService : IAzureBlobService
    {
        //private readonly string _storageAccount = "hirehiveblob";
        //private readonly string _accessKey = "eI42LydCJsBdaT8XtpjfbPBKmz98Gx25yxGVCtbcJPbpGkNbfoNCR/22E5aERjwpfJA+1ToRG2m0+ASt5WiITA==";
        private readonly BlobServiceClient _blobServiceClient;
        public AzureBlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            //var credential = new StorageSharedKeyCredential(_storageAccount, _accessKey);
            //var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            //_blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        }

        public async Task<(string BlobName, string Uri)> UploadFileBlob(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("resumefiles");
            await containerClient.CreateIfNotExistsAsync();

            string fileExtension = Path.GetExtension(file.FileName);
            string blobName = Guid.NewGuid().ToString() + fileExtension;

            var blobClient = containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return (blobClient.Name, blobClient.Uri.ToString());
        }

        public void Delete(string containerName, string blobName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            if (blobClient.Exists())
                blobClient.Delete();
        }
    }
}
