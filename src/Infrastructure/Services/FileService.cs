using Application.Interfaces;
using HireHive.Application.Interfaces;
using HireHive.Domain.Entities;
using HireHive.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HireHive.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly IResumeRepository _resumeRepository;
        public FileService(IAzureBlobService azureBlobService, IResumeRepository resumeRepository)
        {
            _azureBlobService = azureBlobService;
            _resumeRepository = resumeRepository;
        }
        public async Task<(string BlobName, string Uri)> UploadFileAsync(IFormFile file, Guid userId)
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

            await _resumeRepository.AddResumeAsync(resume);

            return (BlobName: blobName, Uri: uri);
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
