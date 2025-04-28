using Azure;
using Azure.Storage.Blobs;
using HireHive.Application.Interfaces;
using HireHive.Domain.Exceptions.Resume;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace HireHive.Infrastructure.FileStorage
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<IAzureBlobService> _logger;
        public AzureBlobService(BlobServiceClient blobServiceClient, ILogger<IAzureBlobService> logger)
        {
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public async Task<string> UploadBlob(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("resumefiles");
            await containerClient.CreateIfNotExistsAsync();

            Guid blobId = Guid.NewGuid();
            string fileExtension = Path.GetExtension(file.FileName);
            string blobName = blobId.ToString() + fileExtension;

            var blobClient = containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();

            try
            {
                await blobClient.UploadAsync(stream, overwrite: true);
                _logger.LogInformation($"Blob {blobName} uploaded successfully.");

                return blobName;
            }
            catch (RequestFailedException)
            {
                _logger.LogError("Failed to upload blob {blobName}.", blobName);
                throw new BlobUploadFailedException();
            }
        }

        public async Task DeleteBlob(string blobName)
        {
            try
            {
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("resumefiles");

                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                if (await blobClient.ExistsAsync())
                    await blobClient.DeleteAsync();
            }
            catch (RequestFailedException)
            {
                _logger.LogError("Failed to delete blob {blobName}.", blobName);
            }
        }
    }
}
