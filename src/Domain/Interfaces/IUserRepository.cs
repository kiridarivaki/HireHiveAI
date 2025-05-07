using HireHive.Domain.Entities;

namespace HireHive.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User>? GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> AddAsync(User user, string password);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}
