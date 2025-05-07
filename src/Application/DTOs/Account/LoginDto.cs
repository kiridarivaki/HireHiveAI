namespace HireHive.Application.DTOs.Account
{
    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Token { get; set; }
        public long ExpiresIn { get; set; }
    }
}
