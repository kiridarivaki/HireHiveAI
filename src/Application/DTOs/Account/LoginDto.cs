namespace HireHive.Application.DTOs.Account
{
    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? TokenType { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public long ExpiresIn { get; set; }
    }
}
