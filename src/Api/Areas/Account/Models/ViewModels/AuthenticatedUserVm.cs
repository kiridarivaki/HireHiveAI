namespace HireHive.Api.Areas.Account.Models.ViewModels
{
    public class AuthenticatedUserVm
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public long ExpiresIn { get; set; }
    }
}
