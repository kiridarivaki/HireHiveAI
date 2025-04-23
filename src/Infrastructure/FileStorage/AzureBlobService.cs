using Azure.Storage;
using Azure.Storage.Blobs;

namespace Infrastructure.FileStorage
{
    public class AzureBlobService
    {
        private readonly string _storageAccount = "hirehiveblob";
        private readonly string _accessKey = "eI42LydCJsBdaT8XtpjfbPBKmz98Gx25yxGVCtbcJPbpGkNbfoNCR/22E5aERjwpfJA+1ToRG2m0+ASt5WiITA==";
        private readonly BlobServiceClient _blobServiceClient;
        public AzureBlobService()
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _accessKey);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            _blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        }
    }
}
