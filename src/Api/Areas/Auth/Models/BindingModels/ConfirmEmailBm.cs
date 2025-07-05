namespace HireHive.Api.Areas.Auth.Models.BindingModels
{
    public class ConfirmEmailBm
    {
        public string Email { get; set; } = null!;
        public string ConfirmationToken { get; set; } = null!;
    }
}
