using HireHive.Domain.Entities;

namespace HireHive.Domain.Interfaces
{
    public interface IResumeRepository
    {
        Task<Resume?> GetResume(Guid resumeId);
        Task<Resume> AddResume(Resume resume);
        Task<Resume> UpdateResume(Resume resume);
        Task<bool> DeleteResume(Guid resumeId);
    }
}
