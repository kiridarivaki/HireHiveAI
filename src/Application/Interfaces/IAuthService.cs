using HireHive.Application.DTOs.Account;

namespace HireHive.Application.Interfaces
{
    public interface IAuthService
    {
        Task Register(RegisterDto user);
        Task<UserInfoDto> GetInfo(Guid userId);
        Task ConfirmEmail(string email, string token);
        Task SendEmailConfirmation(string email);
        Task<AuthenticatedUserDto> Login(LoginDto user);
        Task<AuthenticatedUserDto> RefreshToken(RefreshDto refreshModel);
        Task RevokeToken(string userId);
    }
}
