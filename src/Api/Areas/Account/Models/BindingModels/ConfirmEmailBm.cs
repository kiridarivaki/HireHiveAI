namespace HireHive.Api.Areas.Account.Models.BindingModels
{
    public class ConfirmEmailBm
    {
        public string Email { get; set; } = null!;
        public string ConfirmationCode { get; set; } = null!;
    }
}
