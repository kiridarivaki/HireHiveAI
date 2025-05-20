using HireHive.Domain.Entities;

namespace HireHive.Domain.Interfaces
{
    public interface IResumeRepository
    {
        Task<Resume?> GetByIdAsync(Guid resumeId);
        Task<Resume?> GetByUserIdAsync(Guid userId);
        Task AddAsync(Resume resume);
        Task UpdateAsync(Resume resume);
        Task DeleteAsync(Resume resume);
    }
}
