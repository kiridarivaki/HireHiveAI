using HireHive.Api.Areas.Admin.Models.BindingModels;
using HireHive.Api.Areas.Common.Controllers;
using HireHive.Application.DTOs.Admin;
using HireHive.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireHive.Api.Areas.Admin.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;
        private readonly IMapper _mapper;
        protected AdminController(IMapper mapper,
            ILogger<AdminController> logger,
            IUserService userService,
            IAdminService adminService)
            : base(mapper, logger)
        {
            _userService = userService;
            _adminService = adminService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("assess")]
        public async Task<IActionResult> Assess([FromForm] AssessmentBm assessmentModel)
        {
            try
            {
                var assessmentResult = await _adminService.AssessBatch(_mapper.Map<AssessmentDto>(assessmentModel));
                _logger.LogInformation("Resume assessment done.");

                return Ok(assessmentResult);
            }
            catch (Exception e)
            {
                _logger.LogError("Resume assessment failed. With exception: {message}", e.Message);
                throw;
            }
        }
    }
}
