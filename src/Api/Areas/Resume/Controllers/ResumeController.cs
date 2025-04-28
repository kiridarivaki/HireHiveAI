using FluentValidation;
using HireHive.Api.Areas.Common.Controllers;
using HireHive.Api.Areas.Resume.Models.BindingModels;
using HireHive.Api.Areas.Resume.Models.ViewModels;
using HireHive.Application.DTOs.Resume;
using HireHive.Application.Interfaces;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Exceptions.Resume;
using Microsoft.AspNetCore.Mvc;

namespace HireHive.Api.Areas.Resume.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ApiController
    {
        private readonly IResumeService _resumeService;
        private readonly IValidator<UploadResumeBm> _uploadFileValidator;
        private readonly IValidator<UpdateResumeBm> _updateFileValidator;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<ApiController> _logger;

        public ResumeController(
            IResumeService resumeService,
            IValidator<UploadResumeBm> uploadFileValidator,
            IValidator<UpdateResumeBm> updateFileValidator,
            IUserService userService,
            IMapper mapper,
            ILogger<ApiController> logger)
            : base(mapper, logger)
        {
            _resumeService = resumeService;
            _uploadFileValidator = uploadFileValidator;
            _updateFileValidator = updateFileValidator;
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
            catch (ResumeNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadResume([FromForm] UploadResumeBm uploadModel)
        {
            var validationResult = await _uploadFileValidator.ValidateAsync(uploadModel);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });

                return BadRequest(new { errors });
            }

            try
            {
                var user = await _userService.GetById(uploadModel.UserId);
                var resume = await _resumeService.Upload(_mapper.Map<UploadResumeDto>(uploadModel));

                return Ok(_mapper.Map<UploadResumeVm>(resume));
            }
            catch (ResumeNotFoundException)
            {
                return NotFound();
            }
            catch (BaseException)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("delete/{resumeId}")]
        public async Task<IActionResult> DeleteResume([FromRoute] Guid resumeId)
        {
            try
            {
                await _resumeService.Delete(resumeId);

                return NoContent();
            }
            catch (ResumeNotFoundException)
            {
                return NotFound();
            }
            catch (BaseException)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("update/{userId}")]
        public async Task<IActionResult> UpdateResume([FromRoute] Guid userId, [FromForm] UpdateResumeBm updateResumeBm)
        {
            var validationResult = await _updateFileValidator.ValidateAsync(updateResumeBm);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });

                return BadRequest(new { errors });
            }

            try
            {
                await _resumeService.Update(userId, _mapper.Map<UpdateResumeDto>(updateResumeBm));

                return Ok();
            }
            catch (ResumeNotFoundException)
            {
                return NotFound();
            }
            catch (BaseException)
            {
                throw;
            }
        }
    }
}
