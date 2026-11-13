namespace HireHive.Application.DTOs.Auth
{
    public class RefreshDto
    {
        public string? AccessToken { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
    }
}
