using HireHive.Domain.Entities;

namespace HireHive.Domain.Interfaces
{
    public interface IResumeRepository
    {
        Task<Resume?> GetByIdAsync(Guid resumeId);
        Task AddAsync(Resume resume);
        Task UpdateAsync(Resume resume);
        void Delete(Resume resume);
    }
}
