using HireHive.Domain.Entities;
using HireHive.Domain.Enums;

namespace HireHive.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User>? GetById(Guid id);
        Task<List<User>> GetAll();
        Task<List<User>> GetByIds(List<Guid> userIds);
        Task<User> Add(User user, string password);
        Task Update(User user);
        Task Delete(User user);
        int CountFiltered(JobType jobType);
    }
}
