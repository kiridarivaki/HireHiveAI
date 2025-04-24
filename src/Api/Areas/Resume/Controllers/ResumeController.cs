using Application.Interfaces;
using FluentValidation;
using HireHive.Api.Areas.Resume.Models;
using HireHive.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireHive.Api.Areas.Resume.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IValidator<UploadResumeBm> _fileValidator;
        private readonly IUserService _userService;

        public ResumeController(
            IFileService fileService,
            IValidator<UploadResumeBm> fileValidator,
            IUserService userService)
        {
            _fileService = fileService;
            _fileValidator = fileValidator;
            _userService = userService;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadResume([FromForm] UploadResumeBm resumeModel)
        {
            var user = await _userService.GetById(resumeModel.UserId);
            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }

            var validationResult = await _fileValidator.ValidateAsync(resumeModel);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                return BadRequest(new { errors });
            }

            var (blobName, uri) = await _fileService.UploadFileAsync(resumeModel.File, resumeModel.UserId);

            return Ok(new { blobName, uri });
        }
    }
}
