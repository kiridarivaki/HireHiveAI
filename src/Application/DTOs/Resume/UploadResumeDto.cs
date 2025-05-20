using Microsoft.AspNetCore.Http;

namespace HireHive.Application.DTOs.Resume
{
    public class UploadResumeDto
    {
        public IFormFile File { get; set; } = null!;
        public string? FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
