using HireHive.Domain.Entities;
using HireHive.Domain.Enums;

namespace HireHive.Domain.Interfaces;

public interface IJobRepository
{
    Task<Job?> GetByIdAsync(Guid jobId);
    Task<List<Job>> GetByOwnerIdAsync(Guid ownerId, JobStatus? status = null);
    Task AddAsync(Job job);
    Task UpdateAsync(Job job);
    Task DeleteAsync(Job job);
}
