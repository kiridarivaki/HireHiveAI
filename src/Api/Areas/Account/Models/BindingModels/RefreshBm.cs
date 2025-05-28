namespace HireHive.Api.Areas.Account.Models.BindingModels
{
    public class RefreshBm
    {
        public string? AccessToken { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
    }
}
