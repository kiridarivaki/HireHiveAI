using HireHive.Application.DTOs.Job;

namespace HireHive.Application.Interfaces;

public interface IJobAssessmentService
{
    Task<JobAssessmentDto> Assess(Guid ownerId, Guid jobId, Guid userId, bool force = false);
    Task<List<JobAssessmentDto>> GetByJob(Guid ownerId, Guid jobId);
}
