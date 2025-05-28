namespace HireHive.Api.Areas.Account.Models.ViewModels
{
    public class AuthenticatedUserVm
    {
        public string? AccessToken { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
        public int ExpiresIn { get; set; }
        public Guid? UserId { get; set; } = null!;
    }
}
