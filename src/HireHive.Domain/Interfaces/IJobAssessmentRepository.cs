using HireHive.Domain.Entities;

namespace HireHive.Domain.Interfaces;

public interface IJobAssessmentRepository
{
    Task<JobAssessment?> GetAsync(Guid jobId, Guid userId);
    Task<List<JobAssessment>> GetByJobIdAsync(Guid jobId);
    Task AddAsync(JobAssessment assessment);
    Task UpdateAsync(JobAssessment assessment);
}
