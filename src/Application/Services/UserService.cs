using Application.DTOs;
using Application.Interfaces;
using Ardalis.GuardClauses;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            ILogger<UserService> logger,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("", $"User with ID {id} was not found.");
            }
            _logger.LogInformation("User with id {id} deleted.", id);

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

        public async Task UpdateUser(Guid id, UpdateUserDto userModel)
        {
            var userToUpdate = await _userRepository.GetByIdAsync(id);
            if (userToUpdate == null)
            {
                throw new NotFoundException("", $"User with ID {id} was not found.");
            }

            userToUpdate.UpdateUser(userModel.FirstName, userModel.LastName);
            await _userRepository.UpdateUserAsync(userToUpdate);
            _logger.LogInformation("User with id {id} updated.", id);
        }
    }
}
