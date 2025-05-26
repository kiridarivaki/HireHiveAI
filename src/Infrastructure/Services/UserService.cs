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
        private readonly AiAssessmentService _aiService;

        public UserService(
            IUserRepository userRepository,
            ILogger<UserService> logger,
            IMapper mapper,
            AiAssessmentService aiService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _aiService = aiService;
        }

        public async Task<List<UserDto>> GetAll()
        {
            var users = await _userRepository.GetAll();
            return _mapper.Map<List<UserDto>>(users);
        }
        public async Task<List<UsersDto>> GetByIds(List<Guid> userIds)
        {
            var users = await _userRepository.GetByIds(userIds);
            return _mapper.Map<List<UsersDto>>(users);
        }

        public async Task<UserDto> GetById(Guid id)
        {
            try
            {
                var user = await _userRepository.GetById(id)
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
                var userToUpdate = await _userRepository.GetById(id)
                    ?? throw new UserNotFoundException();

                userToUpdate.UpdateUser(userModel.FirstName, userModel.LastName, userModel.EmploymentStatus, userModel.JobTypes);

                await _userRepository.Update(userToUpdate);

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
                var user = await _userRepository.GetById(id)
                                ?? throw new UserNotFoundException();

                await _userRepository.Delete(user);
                _logger.LogInformation("User {id} deleted.", id);
            }
            catch (BaseException)
            {
                _logger.LogWarning("Error deleting user {id}.", id);
                throw;
            }

        }
    }
}
