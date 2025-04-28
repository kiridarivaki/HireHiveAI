using FluentValidation;
using HireHive.Api.Areas.Account.Models.BindingModels;
using HireHive.Api.Areas.Common.Controllers;
using HireHive.Application.DTOs.Account;
using HireHive.Application.Interfaces;
using HireHive.Domain.Exceptions;
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
        var validationResult = await _registerValidator.ValidateAsync(registerModel);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });

            return BadRequest(new { errors });
        }

        try
        {
            await _authService.Register(_mapper.Map<RegisterDto>(registerModel));

            return Ok();
        }
        catch (BaseException)
        {
            throw;
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginBm loginModel)
    {
        await _authService.Login(_mapper.Map<LoginDto>(loginModel));
        // todo: change return to unauthorized based on service output 
        return Ok();
    }
}
