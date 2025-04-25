using Microsoft.AspNetCore.Http;

namespace HireHive.Application.Interfaces
{
    public interface IAzureBlobService
    {
        Task<string> UploadBlob(IFormFile file);
        Task DeleteBlob(string blobName);
    }
}
