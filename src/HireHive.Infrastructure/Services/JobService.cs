using HireHive.Application.DTOs.Job;
using HireHive.Application.Interfaces;
using HireHive.Domain.Entities;
using HireHive.Domain.Enums;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Exceptions.Job;
using HireHive.Domain.Exceptions.Resume;
using HireHive.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HireHive.Infrastructure.Services;

public class JobService(
    IJobRepository jobRepository,
    IResumeRepository resumeRepository,
    IUserRepository userRepository,
    ILogger<JobService> logger,
    IMapper mapper) : IJobService
{
    private readonly IJobRepository _jobRepository = jobRepository;
    private readonly IResumeRepository _resumeRepository = resumeRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<JobService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<JobDto> Create(Guid ownerId, CreateJobDto createDto)
    {
        var criteria = _mapper.Map<List<JobCriterion>>(createDto.Criteria);
        var job = new Job(createDto.Title, createDto.Description, createDto.JobType, criteria, ownerId);

        await _jobRepository.AddAsync(job);
        _logger.LogInformation("Job {id} created by user {ownerId}.", job.Id, ownerId);

        return _mapper.Map<JobDto>(job);
    }

    public async Task<List<JobDto>> GetAll(Guid ownerId, JobStatus? status = null)
    {
        var jobs = await _jobRepository.GetByOwnerIdAsync(ownerId, status);

        return _mapper.Map<List<JobDto>>(jobs);
    }

    public async Task<JobDto> GetById(Guid ownerId, Guid jobId)
    {
        try
        {
            var job = await GetOwnedJob(ownerId, jobId);

            return _mapper.Map<JobDto>(job);
        }
        catch (BaseException)
        {
            _logger.LogWarning("Job {id} not found for user {ownerId}.", jobId, ownerId);
            throw;
        }
    }

    public async Task Update(Guid ownerId, Guid jobId, UpdateJobDto updateDto)
    {
        try
        {
            var job = await GetOwnedJob(ownerId, jobId);

            var criteria = updateDto.Criteria == null
                ? null
                : _mapper.Map<List<JobCriterion>>(updateDto.Criteria);

            job.Update(updateDto.Title, updateDto.Description, updateDto.JobType, criteria, updateDto.Status);

            await _jobRepository.UpdateAsync(job);
            _logger.LogInformation("Job {id} updated.", jobId);
        }
        catch (BaseException)
        {
            _logger.LogWarning("Error updating job {id}.", jobId);
            throw;
        }
    }

    public async Task Delete(Guid ownerId, Guid jobId)
    {
        try
        {
            var job = await GetOwnedJob(ownerId, jobId);

            await _jobRepository.DeleteAsync(job);
            _logger.LogInformation("Job {id} deleted.", jobId);
        }
        catch (BaseException)
        {
            _logger.LogWarning("Error deleting job {id}.", jobId);
            throw;
        }
    }

    public async Task<List<Guid>> GetApplicantIds(Guid ownerId, Guid jobId, int skip, int take)
    {
        var job = await GetOwnedJob(ownerId, jobId);

        var resumes = await _resumeRepository.GetResumesToAssessAsync(job.JobType, skip, take);

        return [.. resumes.Select(r => r.UserId)];
    }

    public async Task<string> GetApplicantResumeText(Guid ownerId, Guid jobId, Guid userId)
    {
        var job = await GetOwnedJob(ownerId, jobId);

        var user = await _userRepository.GetById(userId)!;
        if (user.JobTypes == null || !user.JobTypes.Contains(job.JobType))
            throw new ApplicantNotFoundException();

        var resume = await _resumeRepository.GetByUserIdAsync(userId)
            ?? throw new ResumeNotFoundException();
        if (string.IsNullOrWhiteSpace(resume.Text))
            throw new ResumeNotProcessedException();

        return resume.Text;
    }

    private async Task<Job> GetOwnedJob(Guid ownerId, Guid jobId)
    {
        var job = await _jobRepository.GetByIdAsync(jobId);
        if (job == null || job.OwnerId != ownerId)
            throw new JobNotFoundException();

        return job;
    }
}
