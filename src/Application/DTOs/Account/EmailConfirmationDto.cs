namespace HireHive.Application.DTOs.Account
{
    public class EmailConfirmationDto
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public long ExpiresIn { get; set; }
    }
}
