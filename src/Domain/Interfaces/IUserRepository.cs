using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<Guid> AddUserAsync(User user, string password);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid id);
    }
}
