using HireHive.Application.DTOs.Account;

namespace HireHive.Application.Interfaces
{
    public interface IAuthService
    {
        Task Register(RegisterDto user);
        Task<UserInfoDto> GetInfo(Guid userId);
        Task ConfirmEmail(string email, string token);
        Task<LoginDto> Login(LoginDto user);
    }
}
