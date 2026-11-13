using HireHive.Application.DTOs.Job;
using HireHive.Domain.Enums;

namespace HireHive.Application.Interfaces;

public interface IJobService
{
    Task<JobDto> Create(Guid ownerId, CreateJobDto createDto);
    Task<List<JobDto>> GetAll(Guid ownerId, JobStatus? status = null);
    Task<JobDto> GetById(Guid ownerId, Guid jobId);
    Task Update(Guid ownerId, Guid jobId, UpdateJobDto updateDto);
    Task Delete(Guid ownerId, Guid jobId);
    Task<List<Guid>> GetApplicantIds(Guid ownerId, Guid jobId, int skip, int take);
    Task<string> GetApplicantResumeText(Guid ownerId, Guid jobId, Guid userId);
}
