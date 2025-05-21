using HireHive.Application.DTOs.Account;
using HireHive.Domain.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace HireHive.Application.Interfaces
{
    public interface ITokenService
    {
        Task<AccessTokenResponse> GenerateToken(User user);
        string GenerateRefreshToken();
        Task<EmailConfirmationDto> GenerateEmailConfirmationToken(User user);
    }
}
