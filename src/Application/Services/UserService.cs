using Application.DTOs;
using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IValidator<UserDto> _validator;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
            ILogger<UserService> logger,
            IValidator<UserDto> validator,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _validator = validator;
            _mapper = mapper;
        }
        public async Task AddUser(UserDto user)
        {
            var validationResult = _validator.Validate(user);
            if (!validationResult.IsValid)
            {
                var errorDetails = new HttpValidationProblemDetails(validationResult.ToDictionary());
                throw new BadRequestExeption(errorDetails);
            }

            bool userExists = await _userRepository.UserExistsAsync(user.Email);
            if (userExists)
            {
                throw new ArgumentException("A user with email {Email} already exists.", user.Email);
            }

            var id = await _userRepository.AddUserAsync(_mapper.Map<DomainUser>(user));
            _logger.LogInformation("User with id {id} created.", id);
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("", $"User with ID {id} was not found.");
            }

            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("", $"User with ID {id} was not found.");
            }
            else
            {
                return _mapper.Map<UserDto>(user);
            }
        }

        public async Task UpdateUser(Guid id, UserDto user)
        {
            var userToUpdate = await _userRepository.GetByIdAsync(id);
            if (userToUpdate == null)
            {
                throw new NotFoundException("", $"User with ID {id} was not found.");
            }

            await _userRepository.UpdateUserAsync(userToUpdate);
        }
    }
}
