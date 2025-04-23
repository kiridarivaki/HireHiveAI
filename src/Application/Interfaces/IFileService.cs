using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IFileService
    {
        Task<(string BlobName, Uri Uri)> UploadFileAsync(IFormFile file, Guid userId);
        Task UpdateFileAsync(Guid fileId, IFormFile file, Guid userId);
        Task<bool> DeleteFileAsync(Guid fileId, Guid userId);
    }
}
