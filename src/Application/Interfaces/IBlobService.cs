using Microsoft.AspNetCore.Http;

namespace HireHive.Application.Interfaces
{
    public interface IBlobService
    {
        Task<string> Upload(IFormFile file);
        Task Delete(string blobName);
        string GetSasUrl(string blobName);
        Task<Stream> GetFileStream(string blobName);
    }
}
