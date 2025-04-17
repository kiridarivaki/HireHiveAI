using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsers();
            if (users == null)
            {
                return NotFound("No users found!");
            }

            return Ok(users);
        }
        
        [HttpGet]
        [Route ("/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            return Ok(user);
        }
    }
}
