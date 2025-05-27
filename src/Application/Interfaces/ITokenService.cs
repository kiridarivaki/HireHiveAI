using HireHive.Domain.Entities;

namespace HireHive.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(User user);
        string GenerateRefreshToken();
        Task<string> GenerateEmailConfirmationToken(User user);
    }
}
