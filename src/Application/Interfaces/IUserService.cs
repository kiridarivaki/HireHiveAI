using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<DomainUser>> GetAllUsers();
        Task<DomainUser> GetUserById(Guid id);
        Task AddUser(Guid id);
        Task UpdateUser(Guid id);
        Task DeleteUser(Guid id);
    }
}
