using Api.Areas.User.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.User.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterUserBindingModel> _registerValidator;
        private readonly IMapper _mapper;
        public AuthController(
            IAuthService authService,
            IValidator<RegisterUserBindingModel> registerValidator,
            IMapper mapper)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserBindingModel userModel)
        {
            var validationResult = await _registerValidator.ValidateAsync(userModel);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                return BadRequest(new { errors });
            }

            var userId = await _authService.RegisterUser(_mapper.Map<RegisterUserDto>(userModel));

            return CreatedAtAction(
                actionName: "GetUserById",
                controllerName: "User",
                routeValues: new { id = userId },
                value: new { id = userId }
            );

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginBindingModel loginModel)
        {
            await _authService.Login(_mapper.Map<LoginUserDto>(loginModel));
            // todo: change return to unauthorized based on service output 
            return Ok();
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();

            return NoContent();
        }
    }
}
