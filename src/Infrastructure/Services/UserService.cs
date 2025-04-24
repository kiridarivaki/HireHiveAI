using Ardalis.GuardClauses;
using HireHive.Application.DTOs.User;
using HireHive.Application.Interfaces;
using HireHive.Domain.Exceptions;
using HireHive.Domain.Exceptions.User;
using HireHive.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HireHive.Infrastructure.Services
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

        public async Task Delete(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id)
                                ?? throw new UserNotFoundException();

                _logger.LogInformation("User with id {id} deleted.", id);

                await _userRepository.DeleteUserAsync(id);
            }
            catch (BaseException ex)
            {
                //todo log
                throw;
            }

        }

        public async Task<List<UserDto>> GetAll()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("", $"User with ID {id} was not found.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task Update(Guid id, UpdateDto userModel)
        {
            var userToUpdate = await _userRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("", $"User with ID {id} was not found.");

            userToUpdate.UpdateUser(userModel.FirstName, userModel.LastName);
            await _userRepository.UpdateUserAsync(userToUpdate);

            _logger.LogInformation("User with id {id} updated.", id);
        }
    }
}
