namespace HireHive.Api.Areas.Account.Models.ViewModels
{
    public class LoginVm
    {
        public string? Token { get; set; }
        public long ExpiresIn { get; set; }
    }
}
