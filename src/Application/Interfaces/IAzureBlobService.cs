using Microsoft.AspNetCore.Http;

namespace HireHive.Application.Interfaces
{
    public interface IAzureBlobService
    {
        Task<string> Upload(IFormFile file);
        Task Delete(string blobName);
        Task<MemoryStream> GetBlobStream(string blobName);
    }
}
