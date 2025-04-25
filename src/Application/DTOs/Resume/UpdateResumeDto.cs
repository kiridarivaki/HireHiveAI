using Microsoft.AspNetCore.Http;

namespace HireHive.Application.DTOs.Resume
{
    public class UpdateResumeDto
    {
        public IFormFile File { get; set; } = null!;
    }
}
