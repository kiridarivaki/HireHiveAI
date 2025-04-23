using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<DomainUser> GetByIdAsync(Guid id);
        Task<bool> UserExistsAsync(string email);
        Task<IEnumerable<DomainUser>> GetAllUsersAsync();
        Task<Guid> AddUserAsync(DomainUser user);
        Task UpdateUserAsync(DomainUser user);
        Task DeleteUserAsync(Guid id);
    }
}
