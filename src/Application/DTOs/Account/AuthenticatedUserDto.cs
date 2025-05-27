namespace HireHive.Application.DTOs.Account
{
    public class AuthenticatedUserDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
