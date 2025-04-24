using HireHive.Domain.Entities;

namespace HireHive.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User>? GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> AddUserAsync(User user, string password);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid id);
    }
}
