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
        private readonly TokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            TokenService tokenService,
            IEmailService emailService,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailService = emailService;
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

                    //await _emailService.SendEmailConfirmationAsync(newUser.Email!, newUser.Id, encodedToken);

                    scope.Complete();
                }
                catch (BaseException)
                {
                    _logger.LogWarning("Registration failed for email {email}.", registerDto.Email);
                    throw;
                }
            }
        }

        public async Task<bool> ValidateUserCredentials(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            return await _userManager.CheckPasswordAsync(user, password);
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

                if (!await _userManager.IsEmailConfirmedAsync(_mapper.Map<User>(loginDto)))
                    throw new UnauthorizedAccessException("Email addresss is not confirmed.");

                bool isValid = await ValidateUserCredentials(loginDto.Email, loginDto.Password);
                if (!isValid)
                    throw new UnauthorizedAccessException("Invalid credentials.");

                var token = _tokenService.GenerateToken(user.Id, user.Email!);
                _logger.LogInformation("User {email} successfully logged in.", loginDto.Email);

                loginDto.Token = token;

                return loginDto;
            }
            catch (BaseException e)
            {
                _logger.LogWarning("Login failed for user {email}. With exception: {message}", loginDto.Email, e.Message);
                throw;
            }
        }
    }
}
