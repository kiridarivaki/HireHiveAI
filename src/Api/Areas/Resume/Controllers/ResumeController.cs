using FluentValidation;
using HireHive.Api.Areas.Common.Controllers;
using HireHive.Api.Areas.Resume.Models.BindingModels;
using HireHive.Api.Areas.Resume.Models.ViewModels;
using HireHive.Application.DTOs.Resume;
using HireHive.Application.Interfaces;
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
        [Route("{resumeId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid resumeId)
        {
            try
            {
                var resume = await _resumeService.GetById(resumeId);

                return Ok(_mapper.Map<ResumeVm>(resume));
            }
            catch
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
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("delete/{resumeId}")]
        public async Task<IActionResult> DeleteResume([FromRoute] Guid resumeId)
        {
            try
            {
                await _resumeService.Delete(resumeId);
            }
            catch (ResumeNotFoundException)
            {
                return NotFound();
            }
            catch (BlobUploadFailedException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPut]
        [Route("update/{resumeId}")]
        public async Task<IActionResult> UpdateResume([FromRoute] Guid resumeId, [FromForm] UpdateResumeBm updateResumeBm)
        {
            var validationResult = await _updateFileValidator.ValidateAsync(updateResumeBm);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });

                return BadRequest(new { errors });
            }

            try
            {
                await _resumeService.Update(resumeId, _mapper.Map<UpdateResumeDto>(updateResumeBm));
            }
            catch (ResumeNotFoundException)
            {
                return NotFound();
            }
            catch (BlobUploadFailedException)
            {
                throw new BlobUploadFailedException();
            }

            return Ok();
        }
    }
}
