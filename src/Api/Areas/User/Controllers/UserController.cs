using Ardalis.GuardClauses;
using FluentValidation;
using HireHive.Api.Areas.Common.Controllers;
using HireHive.Api.Areas.User.Models;
using HireHive.Application.DTOs.User;
using HireHive.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireHive.Api.Areas.User.Controllers;


public class UserController : ApiController
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;
    private readonly IValidator<UpdateBm> _updateValidator;

    public UserController(
        IUserService userService,
        IValidator<UpdateBm> updateValidator,
        IMapper mapper,
        ILogger<UserController> logger)
        : base(mapper, logger)
    {
        _userService = userService;
        _updateValidator = updateValidator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _userService.GetAll();

        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            var user = await _userService.GetById(id);

            return Ok(user);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBm updateUserBm)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateUserBm);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
            return BadRequest(new { errors });
        }

        await _userService.Update(id, _mapper.Map<UpdateDto>(updateUserBm));

        return Ok();
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _userService.Delete(id);

        return NoContent();
    }
}
