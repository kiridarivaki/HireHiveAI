using Azure.Storage.Blobs;
using HireHive.Application.Interfaces;
using Microsoft.AspNetCore.Http;


namespace HireHive.Infrastructure.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _blobSasToken;
        public AzureBlobService(BlobServiceClient blobServiceClient, string blobSasToken)
        {
            _blobServiceClient = blobServiceClient;
            _blobSasToken = blobSasToken;
        }

        public async Task<string> Upload(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("resumefiles");
            await containerClient.CreateIfNotExistsAsync();

            Guid blobId = Guid.NewGuid();
            string fileExtension = Path.GetExtension(file.FileName);
            string blobName = blobId.ToString() + fileExtension;

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

        public async Task<MemoryStream> GetBlobStream(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("resumefiles");
            var blobClient = containerClient.GetBlobClient(blobName);
            MemoryStream documentStream = new MemoryStream();

            await blobClient.DownloadToAsync(documentStream);
            documentStream.Position = 0;

            return documentStream;
        }

    }
}
