using HireHive.Application.DTOs.User;

namespace HireHive.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAll();
        Task<UserDto> GetById(Guid id);
        Task Update(Guid id, UpdateDto user);
        Task Delete(Guid id);
        Task GetAllPaginated();
    }
}
