namespace HireHive.Api.Areas.Account.Models.ViewModels
{
    public class EmailConfirmationVm
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public long ExpiresIn { get; set; }
    }
}
