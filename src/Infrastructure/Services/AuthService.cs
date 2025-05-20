using HireHive.Application.DTOs.Account;
using HireHive.Application.Interfaces;
using HireHive.Domain.Entities;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Exceptions.User;
using HireHive.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Transactions;

namespace HireHive.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailService,
            ITokenService tokenService,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _tokenService = tokenService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task Register(RegisterDto registerDto)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                                             new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                                             TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(registerDto.Email);
                    if (user != null)
                        throw new ArgumentException("A user with email {Email} already exists.", registerDto.Email);

                    var newUser = new User(registerDto.Email, registerDto.FirstName, registerDto.LastName, registerDto.EmploymentStatus);

                    await _userRepository.AddAsync(newUser, registerDto.Password);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                    await _emailService.SendEmailConfirmationAsync(newUser.Email!, newUser.Id, encodedToken);

                    scope.Complete();
                }
                catch (BaseException)
                {
                    _logger.LogWarning("Registration failed for email {email}.", registerDto.Email);
                    throw;
                }
            }
        }

        public async Task ConfirmEmail(string email, string token)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email)
                    ?? throw new UserNotFoundException();

                var decodedToken = Uri.UnescapeDataString(token);
                var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

                if (!result.Succeeded)
                    throw new Exception("Email confirmation failed.");

                _logger.LogInformation("Confirmed email for user {email}.", email);
            }
            catch (BaseException e)
            {
                _logger.LogWarning("Email confirmation failed for user {email}. With exception: {message}", email, e.Message);
                throw;
            }
        }

        public async Task<LoginDto> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email)
                    ?? throw new UserNotFoundException();

                var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                if (!emailConfirmed)
                    throw new UnauthorizedAccessException("Email addresss is not confirmed.");

                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isPasswordValid)
                    throw new UnauthorizedAccessException("Invalid credentials.");

                var accessTokenResponse = await _tokenService.GenerateToken(user);

                _mapper.Map(accessTokenResponse, loginDto);
                loginDto.UserId = user.Id;

                _logger.LogInformation("Login succeeded for user {email}.", loginDto.Email);

                return loginDto;
            }
            catch (BaseException e)
            {
                _logger.LogWarning("Login failed for user {email}. With exception: {message}", loginDto.Email, e.Message);
                throw;
            }
        }

        public async Task<IList<string>> GetRole(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString())
                    ?? throw new UserNotFoundException();

                var roles = await _userManager.GetRolesAsync(user);
                _logger.LogWarning("Role for user {userId} successfully fetched.", userId);

                return roles;

            }
            catch (Exception e)
            {
                _logger.LogWarning("Failed to get role for user {userId}. With exception: {message}", userId, e.Message);
                throw;
            }
        }
    }
}
