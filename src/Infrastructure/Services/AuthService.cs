using HireHive.Application.DTOs.Account;
using HireHive.Application.Interfaces;
using HireHive.Domain.Entities;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace HireHive.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
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

                    scope.Complete();
                }
                catch (BaseException)
                {
                    _logger.LogWarning("Registration failed for email {email}.", registerDto.Email);
                    throw;
                }
            }
        }

        public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task Login(LoginDto userDto)
        {
            bool isValid = await ValidateUserCredentialsAsync(userDto.Email, userDto.Password);
            if (!isValid)
            {
                _logger.LogWarning("Login failed for user {Email}.", userDto.Email);
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            await _signInManager.SignInAsync(_mapper.Map<User>(userDto), isPersistent: false);
            // todo: add jwt tokens 
        }
    }
}
