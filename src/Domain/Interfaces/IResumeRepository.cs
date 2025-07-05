using HireHive.Domain.Entities;
using HireHive.Domain.Enums;

namespace HireHive.Domain.Interfaces
{
    public interface IResumeRepository
    {
        Task<Resume?> GetByIdAsync(Guid resumeId);
        Task<Resume?> GetByUserIdAsync(Guid userId);
        Task AddAsync(Resume resume);
        Task UpdateAsync(Resume resume);
        Task DeleteAsync(Resume resume);
        List<Resume> GetResumesToAssess(JobType jobType, int skip, int take);
    }
}
