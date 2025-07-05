using HireHive.Domain.Entities;
using System.Security.Claims;

namespace HireHive.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(User user);
        string GenerateRefreshToken();
        Task<string> GenerateEmailConfirmationToken(User user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
