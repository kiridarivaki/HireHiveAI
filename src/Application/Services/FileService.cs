using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class FileService : IFileService
    {
        private readonly IAzureBlobService _azureBlobService;
        public FileService(IAzureBlobService azureBlobService)
        {
            _azureBlobService = azureBlobService;
        }
        public async Task<(string BlobName, Uri Uri)> UploadFileAsync(IFormFile file, Guid userId)
        {
            var (blobName, uri) = await _azureBlobService.UploadFileBlob(file);

            var resume = new Resume
            {
                UserId = userId,
                FileName = file.FileName,
                BlobName = blobName,
                ContentType = file.ContentType,
                FileSize = file.Length,
                LastUpdated = DateTime.UtcNow
            };

            // var resumeId = await _resumeRepository.AddResumeAsync(resume);
        }

        public Task UpdateFileAsync(Guid fileId, IFormFile file, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteFileAsync(Guid fileId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
