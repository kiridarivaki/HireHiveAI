using Microsoft.AspNetCore.Http;

namespace HireHive.Application.Interfaces
{
    public interface IResumeJobService
    {
        Task ProcessResume(IFormFile file, Guid userId);
    }
}
