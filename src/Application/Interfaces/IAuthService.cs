using HireHive.Application.DTOs.Account;

namespace HireHive.Application.Interfaces
{
    public interface IAuthService
    {
        Task Register(RegisterDto user);
        Task<bool> ValidateUserCredentialsAsync(string email, string password);
        Task Login(LoginDto user);
    }
}
