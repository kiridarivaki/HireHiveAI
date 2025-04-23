using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(Guid id);
        Task UpdateUser(Guid id, UpdateUserDto user);
        Task DeleteUser(Guid id);
    }
}
