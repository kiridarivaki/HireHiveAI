using FluentValidation;
using HireHive.Api.Areas.Common.Controllers;
using HireHive.Api.Areas.User.Models.BindingModels;
using HireHive.Api.Areas.User.Models.ViewModels;
using HireHive.Application.DTOs.User;
using HireHive.Application.Interfaces;
using HireHive.Domain.Exceptions.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireHive.Api.Areas.User.Controllers;

[Authorize]
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
        var users = await _userService.GetAll();

        return Ok(_mapper.Map<List<UserVm>>(users));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            var user = await _userService.GetById(id);

            return Ok(_mapper.Map<UserVm>(user));
        }
        catch (UserNotFoundException e)
        {
            _logger.LogError("User {id} not found. With exception: {message}", id, e.Message);
            return NotFound();
        }
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBm updateUserBm)
    {
        try
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
        catch (UserNotFoundException e)
        {
            _logger.LogError("User {id} not found. With exception: {message}", id, e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to update user {id}. With exception: {message}", id, e.Message);
            throw;
        }
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        try
        {
            await _userService.Delete(id);

            return NoContent();
        }
        catch (UserNotFoundException e)
        {
            _logger.LogError("User {id} not found. With exception: {message}", id, e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to delete user {id}. With exception: {message}", id, e.Message);
            throw;
        }
    }
}
