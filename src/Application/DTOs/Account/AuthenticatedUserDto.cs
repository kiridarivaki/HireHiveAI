namespace HireHive.Application.DTOs.Account
{
    public class AuthenticatedUserDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public Guid? UserId { get; set; } = null!;
    }
}
