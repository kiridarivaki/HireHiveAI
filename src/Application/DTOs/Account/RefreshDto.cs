namespace HireHive.Application.DTOs.Account
{
    public class RefreshDto
    {
        public string? AccessToken { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
    }
}
