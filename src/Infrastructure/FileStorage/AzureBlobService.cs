using Azure.Storage;
using Azure.Storage.Blobs;

namespace Infrastructure.FileStorage
{
    public class AzureBlobService
    {
        private readonly string _storageAccount = "hirehiveblob";
        private readonly string _accessKey = "";
        private readonly BlobServiceClient _blobServiceClient;
        public AzureBlobService()
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _accessKey);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            _blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        }
    }
}
