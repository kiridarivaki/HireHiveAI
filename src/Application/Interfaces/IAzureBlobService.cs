using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IAzureBlobService
    {
        Task<(string BlobName, string Uri)> UploadFileBlob(IFormFile file);
        void Delete(string containerName, string blobName);
    }
}
