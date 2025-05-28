using HireHive.Application.DTOs.Account;
using HireHive.Application.Interfaces;
using HireHive.Domain.Entities;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Exceptions.Account;
using HireHive.Domain.Exceptions.User;
using HireHive.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Transactions;

namespace HireHive.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            UserManager<User> userManager,
            IEmailService emailService,
            ITokenService tokenService,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
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
                        throw new ArgumentException("A user with email {email} already exists.", registerDto.Email);

                    var newUser = new User(registerDto.Email, registerDto.FirstName, registerDto.LastName, registerDto.EmploymentStatus, registerDto.JobTypes);

                    await _userRepository.Add(newUser, registerDto.Password);

                    await SendEmailConfirmation(newUser.Email!);

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

                var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
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

        public async Task SendEmailConfirmation(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email)
                    ?? throw new UserNotFoundException();

                var confirmationToken = await _tokenService.GenerateEmailConfirmationToken(user);
                await _emailService.SendConfirmationEmail(email!, confirmationToken);

                _logger.LogInformation("Confirmation email sent to {email}.", email);
            }
            catch (BaseException e)
            {
                _logger.LogWarning("Confirmation email failed to send to {email}. With exception: {message}", email, e.Message);
                throw;
            }
        }

        public async Task<AuthenticatedUserDto> Login(LoginDto loginDto)
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

                var accessToken = await _tokenService.GenerateToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

                await _userManager.UpdateAsync(user);

                var authUser = new AuthenticatedUserDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = (int)TimeSpan.FromMinutes(1).TotalSeconds,
                    UserId = user.Id
                };

                _logger.LogInformation("Login succeeded for user {email}.", loginDto.Email);

                return authUser;
            }
            catch (Exception e)
            {
                _logger.LogWarning("Login failed for user {email}. With exception: {message}", loginDto.Email, e.Message);
                throw;
            }
        }

        public async Task<AuthenticatedUserDto> RefreshToken(RefreshDto refreshModel)
        {
            if (string.IsNullOrWhiteSpace(refreshModel.RefreshToken))
                throw new InvalidRefreshTokenException();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshModel.RefreshToken);

            if (user == null || user.RefreshTokenExpiry <= DateTime.UtcNow)
                throw new InvalidRefreshTokenException();

            var principal = _tokenService.GetPrincipalFromExpiredToken(refreshModel.AccessToken);
            var username = principal.Identity.Name;

            var newAccessToken = await _tokenService.GenerateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            return new AuthenticatedUserDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = (int)TimeSpan.FromMinutes(1).TotalSeconds,
                UserId = user.Id
            };
        }

        public async Task RevokeToken(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId)
                    ?? throw new UserNotFoundException();

                user.RefreshToken = null;
                user.RefreshTokenExpiry = DateTime.MinValue;

                var result = await _userManager.UpdateAsync(user);

                _logger.LogInformation("Token successfully revoked for user {userId}.", userId);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Failed to revoke token for user {userId}. With exception: {message}", userId, e.Message);
                throw;
            }
        }

        public async Task<UserInfoDto> GetInfo(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString())
                    ?? throw new UserNotFoundException();

                var roles = await _userManager.GetRolesAsync(user);

                var authUser = _mapper.Map<UserInfoDto>(user);
                authUser.Roles = roles.ToList();

                _logger.LogInformation("Info for user {userId} successfully fetched.", userId);

                return authUser;
            }
            catch (Exception e)
            {
                _logger.LogWarning("Failed to get info for user {userId}. With exception: {message}", userId, e.Message);
                throw;
            }
        }
    }
}
