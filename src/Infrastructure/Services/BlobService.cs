using Azure.Storage.Blobs;
using HireHive.Application.Interfaces;
using Microsoft.AspNetCore.Http;


namespace HireHive.Infrastructure.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _blobSasToken;
        public BlobService(BlobServiceClient blobServiceClient, string blobSasToken)
        {
            _blobServiceClient = blobServiceClient;
            _blobSasToken = blobSasToken;
        }

        public async Task<string> Upload(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("resumefiles");
            await containerClient.CreateIfNotExistsAsync();

            string fileExtension = Path.GetExtension(file.FileName);
            string blobName = file.FileName.ToString() + fileExtension;

            var blobClient = containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();

            var response = await blobClient.UploadAsync(stream, overwrite: true);

            if (response.GetRawResponse().Status != 201)
                throw new Exception();

            string blobUri = blobClient.Uri.ToString() + "?" + _blobSasToken;

            return blobName;
        }

        public async Task Delete(string blobName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("resumefiles");

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }

        public string GetSasUrl(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("resumefiles");
            var blobClient = containerClient.GetBlobClient(blobName);

            return $"{blobClient.Uri}?{_blobSasToken}";
        }

        public async Task<Stream> GetFileStream(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("resumefiles");
            var blobClient = containerClient.GetBlobClient(blobName);
            var stream = await blobClient.OpenReadAsync();

            return stream;
        }

    }
}
