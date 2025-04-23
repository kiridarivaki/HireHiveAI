using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<Guid> RegisterUser(RegisterUserDto user);
        Task<bool> ValidateUserCredentialsAsync(string email, string password);
        Task Login(LoginUserDto user);
        Task Logout();
    }
}
