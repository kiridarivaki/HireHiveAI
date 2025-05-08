using FluentValidation;
using HireHive.Api.Areas.Admin.Models.BindingModels;
using HireHive.Api.Areas.Common.Controllers;
using HireHive.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireHive.Api.Areas.Admin.Controllers
{
    public class AdminController : ApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<string> _filterValidator;
        private readonly ILogger<AdminController> _logger;
        protected AdminController(IMapper mapper,
            ILogger<AdminController> logger,
            IUserRepository userRepository,
            IValidator<string> filterValidator)
            : base(mapper, logger)
        {
            _userRepository = userRepository;
            _filterValidator = filterValidator;
            _logger = logger;
        }

        [HttpGet]
        [Route("list-candidates")]
        public async Task<IActionResult> ListUsers([FromQuery] ListUsersBm listUsersModel)
        {
            try
            {
                var validationResult = await _filterValidator.ValidateAsync(listUsersModel.jobFilter);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });

                    return BadRequest(new { errors });
                }

                return Ok();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
