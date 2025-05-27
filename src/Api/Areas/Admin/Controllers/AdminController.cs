using HireHive.Api.Areas.Admin.Models.BindingModels;
using HireHive.Api.Areas.Admin.Models.ViewModels;
using HireHive.Api.Areas.Common.Controllers;
using HireHive.Application.DTOs.Admin;
using HireHive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireHive.Api.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : ApiController
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;
        private readonly IMapper _mapper;

        public AdminController(IMapper mapper,
            ILogger<AdminController> logger,
            IUserService userService,
            IAdminService adminService)
            : base(mapper, logger)
        {
            _adminService = adminService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("assess")]
        public async Task<IActionResult> Assess([FromBody] AssessmentParamsBm assessmentModel)
        {
            try
            {
                var assessmentResult = await _adminService.AssessBatch(_mapper.Map<AssessmentParamsDto>(assessmentModel));
                _logger.LogInformation("Resume assessment done.");

                return Ok(_mapper.Map<List<AssessmentResultVm>>(assessmentResult));
            }
            catch (Exception e)
            {
                _logger.LogError("Resume assessment failed. With exception: {message}", e.Message);
                throw;
            }
        }

        [HttpPost]
        [Route("sort")]
        public IActionResult Sort([FromBody] SortDataBm sortDataModel)
        {
            try
            {
                var result = _adminService.SortResults(_mapper.Map<SortDataDto>(sortDataModel));
                _logger.LogInformation("Sorting done.");

                return Ok(_mapper.Map<List<SortResultVm>>(result));
            }
            catch (Exception e)
            {
                _logger.LogError("Sorting failed. With exception: {message}", e.Message);
                throw;
            }
        }
    }
}
