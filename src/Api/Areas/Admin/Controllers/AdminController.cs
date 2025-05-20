using FluentValidation;
using HireHive.Api.Areas.Admin.Models.BindingModels;
using HireHive.Api.Areas.Common.Controllers;
using HireHive.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireHive.Api.Areas.Admin.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IValidator<string> _filterValidator;
        private readonly ILogger<AdminController> _logger;
        protected AdminController(IMapper mapper,
            ILogger<AdminController> logger,
            IUserService userService,
            IValidator<string> filterValidator)
            : base(mapper, logger)
        {
            _userService = userService;
            _filterValidator = filterValidator;
            _logger = logger;
        }

        [HttpGet]
        [Route("list-users")]
        public async Task<IActionResult> ListUsersPaginated([FromQuery] ListUsersBm listUsersModel)
        {
            try
            {
                var validationResult = await _filterValidator.ValidateAsync(listUsersModel.jobFilter);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });

                    return BadRequest(new { errors });
                }

                //var paginatedUsers = await _userService.GetAllPaginated();

                return Ok();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
