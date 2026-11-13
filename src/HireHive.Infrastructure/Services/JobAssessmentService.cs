using HireHive.Application.DTOs.Admin;
using HireHive.Application.DTOs.Job;
using HireHive.Application.Interfaces;
using HireHive.Domain.Entities;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HireHive.Infrastructure.Services;

public class JobAssessmentService(
    IJobService jobService,
    IJobAssessmentRepository jobAssessmentRepository,
    IAssessmentService assessmentService,
    ILogger<JobAssessmentService> logger,
    IMapper mapper) : IJobAssessmentService
{
    private readonly IJobService _jobService = jobService;
    private readonly IJobAssessmentRepository _jobAssessmentRepository = jobAssessmentRepository;
    private readonly IAssessmentService _assessmentService = assessmentService;
    private readonly ILogger<JobAssessmentService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<JobAssessmentDto> Assess(Guid ownerId, Guid jobId, Guid userId, bool force = false)
    {
        try
        {
            var job = await _jobService.GetById(ownerId, jobId);

            var assessment = await _jobAssessmentRepository.GetAsync(jobId, userId);
            if (assessment != null && !force)
                return _mapper.Map<JobAssessmentDto>(assessment);

            var resumeText = await _jobService.GetApplicantResumeText(ownerId, jobId, userId);

            var assessmentParams = new AssessmentParamsDto
            {
                JobDescription = job.Description,
                JobType = job.JobType,
                Criteria = [.. job.Criteria.Select(c => c.Name)],
                CriteriaWeights = [.. job.Criteria.Select(c => c.Weight)]
            };
            var resumeToAssess = new UserResumeDto { UserId = userId, ResumeText = resumeText };

            var results = await _assessmentService.AssessUsers([resumeToAssess], assessmentParams);
            var result = results.FirstOrDefault()
                ?? throw new InvalidOperationException("The AI service returned no assessment.");

            var matchPercentage = Math.Clamp(result.MatchPercentage, 0, 100);

            if (assessment == null)
            {
                assessment = new JobAssessment(jobId, userId, matchPercentage, result.Explanation);
                await _jobAssessmentRepository.AddAsync(assessment);
            }
            else
            {
                assessment.Update(matchPercentage, result.Explanation);
                await _jobAssessmentRepository.UpdateAsync(assessment);
            }

            _logger.LogInformation("Candidate {userId} assessed for job {jobId}.", userId, jobId);

            return _mapper.Map<JobAssessmentDto>(assessment);
        }
        catch (BaseException)
        {
            _logger.LogWarning("Failed to assess candidate {userId} for job {jobId}.", userId, jobId);
            throw;
        }
    }

    public async Task<List<JobAssessmentDto>> GetByJob(Guid ownerId, Guid jobId)
    {
        await _jobService.GetById(ownerId, jobId);

        var assessments = await _jobAssessmentRepository.GetByJobIdAsync(jobId);

        return _mapper.Map<List<JobAssessmentDto>>(assessments);
    }
}
