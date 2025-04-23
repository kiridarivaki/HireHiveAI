using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Guid> RegisterUser(RegisterUserDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            if (user != null)
            {
                throw new ArgumentException("A user with email {Email} already exists.", userDto.Email);
            }

            var newUser = _mapper.Map<User>(userDto);
            newUser.UserName = userDto.Email;

            var id = await _userRepository.AddUserAsync(newUser, userDto.Password);

            return id;
        }

        public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task Login(LoginUserDto userDto)
        {
            bool isValid = await ValidateUserCredentialsAsync(userDto.Email, userDto.Password);
            if (!isValid)
            {
                _logger.LogWarning("Login failed for user {Email}.", userDto.Email);
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            AppUser appUser = _mapper.Map<AppUser>(userDto);
            await _signInManager.SignInAsync(appUser, isPersistent: false);
            // todo: add jwt tokens 
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
