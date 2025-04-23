using Api.Areas.User.Models;
using Application.DTOs;
using Application.Interfaces;
using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.User.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateUserBindingModel> _updateValidator;

        public UserController(
            IUserService userService,
            IMapper mapper,
            IValidator<UpdateUserBindingModel> updateValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetAllUsers();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            try
            {
                var user = await _userService.GetUserById(id);

                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPatch]
        [Route("update/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserBindingModel userModel)
        {
            var validationResult = await _updateValidator.ValidateAsync(userModel);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
                return BadRequest(new { errors });
            }

            await _userService.UpdateUser(id, _mapper.Map<UpdateUserDto>(userModel));

            return NoContent();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            await _userService.DeleteUser(id);

            return NoContent();
        }
    }
}
