using HireHive.Domain.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace HireHive.Application.Interfaces
{
    public interface ITokenService
    {
        Task<AccessTokenResponse> GenerateToken(User user);
        string GenerateRefreshToken();
        Task<string> GenerateEmailConfirmationToken(User user);
    }
}
