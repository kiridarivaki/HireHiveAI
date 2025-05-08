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

        public async Task<List<UserDto>> GetAll()
        {
            var users = await _userRepository.GetAllAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetById(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id)
                    ?? throw new UserNotFoundException();

                return _mapper.Map<UserDto>(user);
            }
            catch (BaseException)
            {
                _logger.LogWarning("User {id} not found.", id);
                throw;
            }
        }

        public async Task Update(Guid id, UpdateDto userModel)
        {
            try
            {
                var userToUpdate = await _userRepository.GetByIdAsync(id)
                    ?? throw new UserNotFoundException();

                userToUpdate.UpdateUser(userModel.FirstName, userModel.LastName);

                await _userRepository.UpdateAsync(userToUpdate);

                _logger.LogInformation("User {id} updated.", id);
            }
            catch (BaseException)
            {
                _logger.LogWarning("Error updating user {id}.", id);
                throw;
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id)
                                ?? throw new UserNotFoundException();

                await _userRepository.DeleteAsync(user);
                _logger.LogInformation("User {id} deleted.", id);
            }
            catch (BaseException)
            {
                _logger.LogWarning("Error deleting user {id}.", id);
                throw;
            }

        }

        public Task GetAllPaginated()
        {
            throw new NotImplementedException();
        }
    }
}
