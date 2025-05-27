using HireHive.Application.Interfaces;
using HireHive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HireHive.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public TokenService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = _configuration["JwtKey"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key!.PadRight((512 / 8), '\0')));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            return accessToken;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> GenerateEmailConfirmationToken(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return encodedToken;
        }
    }
}
