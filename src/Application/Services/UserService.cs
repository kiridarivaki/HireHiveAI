using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public Task AddUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DomainUser>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<UserDto>(users);
        }

        public Task<DomainUser> GetUserById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
