namespace HireHive.Api.Areas.Account.Models.ViewModels
{
    public class LoginVm
    {
        public Guid UserId { get; set; }
        public string? TokenType { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public long ExpiresIn { get; set; }
    }
}
