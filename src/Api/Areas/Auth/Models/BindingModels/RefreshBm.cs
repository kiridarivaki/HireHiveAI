namespace HireHive.Api.Areas.Auth.Models.BindingModels
{
    public class RefreshBm
    {
        public string? AccessToken { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
    }
}
