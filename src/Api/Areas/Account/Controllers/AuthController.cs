using FluentValidation;
using HireHive.Api.Areas.Account.Models.BindingModels;
using HireHive.Api.Areas.Account.Models.ViewModels;
using HireHive.Api.Areas.Common.Controllers;
using HireHive.Application.DTOs.Account;
using HireHive.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireHive.Api.Areas.Account.Controllers;

public class AuthController : ApiController
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterBm> _registerValidator;
    private readonly IMapper _mapper;
    private readonly ILogger<ApiController> _logger;
    public AuthController(
        IAuthService authService,
        IValidator<RegisterBm> registerValidator,
        IMapper mapper,
        ILogger<ApiController> logger)
        : base(mapper, logger)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterBm registerModel)
    {
        try
        {
            var validationResult = await _registerValidator.ValidateAsync(registerModel);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });

                return BadRequest(new { errors });
            }

            await _authService.Register(_mapper.Map<RegisterDto>(registerModel));

            _logger.LogInformation("User with email {email} registered.", registerModel.Email);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to register user {email}, With exception: {message}", registerModel.Email, e.Message);
            throw;
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginBm loginModel)
    {
        try
        {
            var authenticatedUser = await _authService.Login(_mapper.Map<LoginDto>(loginModel));

            _logger.LogInformation("User with email {email} logged in.", loginModel.Email);

            return Ok(_mapper.Map<LoginVm>(authenticatedUser));
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to login user {email}, With exception: {message}", loginModel.Email, e.Message);

            return Unauthorized(new { message = e.Message });
        }
    }

    [HttpPost]
    [Route("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailBm confirmEmailModel)
    {
        try
        {
            await _authService.ConfirmEmail(confirmEmailModel.Email, confirmEmailModel.ConfirmationToken);

            _logger.LogInformation("Email {email} confirmed.", confirmEmailModel.Email);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to confirm email {email}. With exception: {message}", confirmEmailModel.Email, e.Message);
            throw;
        }
    }

    [HttpPost]
    [Route("resend-confirmation")]
    public async Task<IActionResult> ResendConfirmation([FromBody] EmailConfirmationBm emailConfirmationBm)
    {
        try
        {
            await _authService.SendEmailConfirmation(emailConfirmationBm.Email);

            _logger.LogInformation("Email confirmation sent to {email}.", emailConfirmationBm.Email);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError("Email confirmation failed to send to {email}. With exception: {message}", emailConfirmationBm.Email, e.Message);
            throw;
        }
    }

    [HttpGet]
    [Route("get-info/{userId}")]
    public async Task<IActionResult> GetUserInfo([FromRoute] Guid userId)
    {
        try
        {
            var userInfo = await _authService.GetInfo(userId);

            _logger.LogInformation("Fetched user info for user {userId}.", userId);

            return Ok(_mapper.Map<UserInfoVm>(userInfo));
        }
        catch (Exception e)
        {
            _logger.LogError("Fetched user info for user {userId}. With exception: {message}", userId, e.Message);
            throw;
        }
    }
}
