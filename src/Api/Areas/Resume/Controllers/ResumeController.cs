using FluentValidation;
using Hangfire;
using HireHive.Api.Areas.Common.Controllers;
using HireHive.Api.Areas.Resume.Models.BindingModels;
using HireHive.Api.Areas.Resume.Models.ViewModels;
using HireHive.Application.DTOs.Resume;
using HireHive.Application.Interfaces;
using HireHive.Domain.Exceptions.Resume;
using HireHive.Infrastructure.Services.AI;
using Microsoft.AspNetCore.Mvc;

namespace HireHive.Api.Areas.Resume.Controllers
{
    public class ResumeController : ApiController
    {
        private readonly IResumeService _resumeService;
        private readonly IValidator<UploadResumeBm> _uploadFileValidator;
        private readonly IValidator<UpdateResumeBm> _updateFileValidator;
        private readonly IUserService _userService;
        private readonly IResumeJobService _resumeJobService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly AiAssessmentService _aiService;
        private readonly IMapper _mapper;
        private readonly ILogger<ResumeController> _logger;

        public ResumeController(
            IResumeService resumeService,
            IValidator<UploadResumeBm> uploadFileValidator,
            IValidator<UpdateResumeBm> updateFileValidator,
            IResumeJobService resumeJobService,
            IBackgroundJobClient backgroundJobClient,
            AiAssessmentService aiService,
            IUserService userService,
            IMapper mapper,
            ILogger<ResumeController> logger)
            : base(mapper, logger)
        {
            _resumeService = resumeService;
            _uploadFileValidator = uploadFileValidator;
            _updateFileValidator = updateFileValidator;
            _resumeJobService = resumeJobService;
            _backgroundJobClient = backgroundJobClient;
            _aiService = aiService;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetByUserId([FromRoute] Guid userId)
        {
            try
            {
                var resume = await _resumeService.GetByUserId(userId);

                return Ok(_mapper.Map<ResumeVm>(resume));
            }
            catch (Exception e)
            {
                _logger.LogError("Resume for user {userId} not found. With exception: {message}", userId, e.Message);
                return NotFound();
            }
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadResumeBm uploadModel)
        {
            try
            {
                var validationResult = await _uploadFileValidator.ValidateAsync(uploadModel);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });

                    return BadRequest(new { errors });
                }

                var user = await _userService.GetById(uploadModel.UserId);
                var resume = await _resumeService.Upload(_mapper.Map<UploadResumeDto>(uploadModel));


                var fileProcessingJob = _backgroundJobClient.Enqueue(() => _resumeJobService.ProcessResume(uploadModel.File, uploadModel.UserId));

                _logger.LogInformation("Resume processing started for user {userId}.", uploadModel.UserId);

                return Ok(_mapper.Map<UploadResumeVm>(resume));
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to upload resume for user {userId}. With exception: {message}", uploadModel.UserId, e.Message);
                throw;
            }
        }

        [HttpDelete]
        [Route("delete/{resumeId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid resumeId)
        {
            try
            {
                await _resumeService.Delete(resumeId);

                return NoContent();
            }
            catch (ResumeNotFoundException e)
            {
                _logger.LogError("Resume {resumeId} not found. With exception: {message}", resumeId, e.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to delete {resumeId}. With exception: {message}", resumeId, e.Message);
                throw;
            }
        }

        [HttpPut]
        [Route("update/{userId}")]
        public async Task<IActionResult> Update([FromRoute] Guid userId, [FromForm] UpdateResumeBm updateResumeBm)
        {
            try
            {
                var validationResult = await _updateFileValidator.ValidateAsync(updateResumeBm);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });

                    return BadRequest(new { errors });
                }

                await _resumeService.Update(userId, _mapper.Map<UpdateResumeDto>(updateResumeBm));

                return Ok();
            }
            catch (ResumeNotFoundException e)
            {
                _logger.LogError("Resume for user {userId} not found. With exception: {message}", userId, e.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to update resume of user {userId}. With exception: {message}", userId, e.Message);
                throw;
            }
        }

        [HttpPost]
        [Route("process")]
        public IActionResult Process([FromForm] ProcessResumeBm resumeModel)
        {
            try
            {
                var fileProcessingJob = _backgroundJobClient.Enqueue(() => _resumeJobService.ProcessResume(resumeModel.File, resumeModel.UserId));

                _logger.LogInformation("Resume processing started for user {userId}.", resumeModel.UserId);

                return Ok();
            }
            catch (ResumeNotFoundException e)
            {
                _logger.LogError("Resume for user {userId} not found. With exception: {message}", resumeModel.UserId, e.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to process resume for user {userId}. With exception: {message}", resumeModel.UserId, e.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("assess/{userId}")]
        public async Task<IActionResult> Assess([FromRoute] Guid userId)
        {
            try
            {
                //todo : add to hangfire job 
                await _aiService.Chat(userId);

                _logger.LogInformation("Resume assessment started for user {userId}.", userId);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("Resume for user {userId} not found. With exception: {message}", userId, e.Message);

                return NotFound();
            }
        }
    }
}
