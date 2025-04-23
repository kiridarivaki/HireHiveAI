using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(Guid id);
        Task AddUser(UserDto user);
        Task UpdateUser(Guid id, UserDto user);
        Task DeleteUser(Guid id);
    }
}
