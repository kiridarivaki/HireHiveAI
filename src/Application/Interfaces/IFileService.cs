using Microsoft.AspNetCore.Http;

namespace HireHive.Application.Interfaces
{
    public interface IFileService
    {
        Task<(string BlobName, string Uri)> UploadFileAsync(IFormFile file, Guid userId);
        Task UpdateFileAsync(Guid fileId, IFormFile file, Guid userId);
        Task<bool> DeleteFileAsync(Guid fileId, Guid userId);
    }
}
