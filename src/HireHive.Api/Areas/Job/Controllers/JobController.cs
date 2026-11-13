using FluentValidation;
using HireHive.Api.Areas.Common.Controllers;
using HireHive.Api.Areas.Job.Models.BindingModels;
using HireHive.Api.Areas.Job.Models.ViewModels;
using HireHive.Application.DTOs.Job;
using HireHive.Application.Interfaces;
using HireHive.Domain.Enums;
using HireHive.Domain.Exceptions.Job;
using HireHive.Domain.Exceptions.Resume;
using HireHive.Domain.Exceptions.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireHive.Api.Areas.Job.Controllers;

[Authorize(Roles = "Admin")]
public class JobController(
    IJobService jobService,
    IJobAssessmentService jobAssessmentService,
    IValidator<CreateJobBm> createValidator,
    IValidator<UpdateJobBm> updateValidator,
    IMapper mapper,
    ILogger<JobController> logger) : ApiController(mapper, logger)
{
    private readonly IJobService _jobService = jobService;
    private readonly IJobAssessmentService _jobAssessmentService = jobAssessmentService;
    private readonly IValidator<CreateJobBm> _createValidator = createValidator;
    private readonly IValidator<UpdateJobBm> _updateValidator = updateValidator;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<JobController> _logger = logger;

    private Guid OwnerId => Guid.Parse(UserId!);

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAll([FromQuery] JobStatus? status)
    {
        var jobs = await _jobService.GetAll(OwnerId, status);

        return Ok(_mapper.Map<List<JobVm>>(jobs));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            var job = await _jobService.GetById(OwnerId, id);

            return Ok(_mapper.Map<JobVm>(job));
        }
        catch (JobNotFoundException e)
        {
            _logger.LogError("Job {id} not found. With exception: {message}", id, e.Message);
            return NotFound();
        }
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create([FromBody] CreateJobBm createJobBm)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(createJobBm);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });

                return BadRequest(new { errors });
            }

            var job = await _jobService.Create(OwnerId, _mapper.Map<CreateJobDto>(createJobBm));

            return Ok(_mapper.Map<JobVm>(job));
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to create job. With exception: {message}", e.Message);
            throw;
        }
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateJobBm updateJobBm)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(updateJobBm);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });

                return BadRequest(new { errors });
            }

            await _jobService.Update(OwnerId, id, _mapper.Map<UpdateJobDto>(updateJobBm));

            return Ok();
        }
        catch (JobNotFoundException e)
        {
            _logger.LogError("Job {id} not found. With exception: {message}", id, e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to update job {id}. With exception: {message}", id, e.Message);
            throw;
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            await _jobService.Delete(OwnerId, id);

            return NoContent();
        }
        catch (JobNotFoundException e)
        {
            _logger.LogError("Job {id} not found. With exception: {message}", id, e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to delete job {id}. With exception: {message}", id, e.Message);
            throw;
        }
    }

    [HttpGet]
    [Route("{id}/applicants")]
    public async Task<IActionResult> GetApplicants([FromRoute] Guid id, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        try
        {
            var applicantIds = await _jobService.GetApplicantIds(OwnerId, id, Math.Max(skip, 0), Math.Clamp(take, 1, 100));

            return Ok(applicantIds);
        }
        catch (JobNotFoundException e)
        {
            _logger.LogError("Job {id} not found. With exception: {message}", id, e.Message);
            return NotFound();
        }
    }

    [HttpGet]
    [Route("{id}/applicants/{userId}/resume")]
    public async Task<IActionResult> GetApplicantResume([FromRoute] Guid id, [FromRoute] Guid userId)
    {
        try
        {
            var text = await _jobService.GetApplicantResumeText(OwnerId, id, userId);

            return Ok(new ApplicantResumeVm { UserId = userId, Text = text });
        }
        catch (Exception e) when (e is JobNotFoundException or ApplicantNotFoundException or UserNotFoundException or ResumeNotFoundException)
        {
            _logger.LogError("Failed to get resume of candidate {userId} for job {id}. With exception: {message}", userId, id, e.Message);
            return NotFound();
        }
        catch (ResumeNotProcessedException e)
        {
            _logger.LogError("Resume of candidate {userId} is not processed yet. With exception: {message}", userId, e.Message);
            return Conflict(new { message = "The resume is still being processed. Try again later." });
        }
    }

    [HttpGet]
    [Route("{id}/assessments")]
    public async Task<IActionResult> GetAssessments([FromRoute] Guid id)
    {
        try
        {
            var assessments = await _jobAssessmentService.GetByJob(OwnerId, id);

            return Ok(_mapper.Map<List<JobAssessmentVm>>(assessments));
        }
        catch (JobNotFoundException e)
        {
            _logger.LogError("Job {id} not found. With exception: {message}", id, e.Message);
            return NotFound();
        }
    }

    [HttpPost]
    [Route("{id}/assess/{userId}")]
    public async Task<IActionResult> Assess([FromRoute] Guid id, [FromRoute] Guid userId, [FromQuery] bool force = false)
    {
        try
        {
            var assessment = await _jobAssessmentService.Assess(OwnerId, id, userId, force);

            return Ok(_mapper.Map<JobAssessmentVm>(assessment));
        }
        catch (Exception e) when (e is JobNotFoundException or ApplicantNotFoundException or UserNotFoundException or ResumeNotFoundException)
        {
            _logger.LogError("Failed to assess candidate {userId} for job {id}. With exception: {message}", userId, id, e.Message);
            return NotFound();
        }
        catch (ResumeNotProcessedException e)
        {
            _logger.LogError("Resume of candidate {userId} is not processed yet. With exception: {message}", userId, e.Message);
            return Conflict(new { message = "The resume is still being processed. Try again later." });
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to assess candidate {userId} for job {id}. With exception: {message}", userId, id, e.Message);
            throw;
        }
    }
}
