using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<DomainUser> GetByIdAsync(Guid id);
        Task<IEnumerable<DomainUser>> GetAllUsersAsync();
        Task AddUser(DomainUser user);
        Task UpdateUser(DomainUser user);
        Task DeleteUser(Guid id);
    }
}
