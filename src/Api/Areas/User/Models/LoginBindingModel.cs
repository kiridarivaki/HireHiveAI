namespace Api.Areas.User.Models
{
    public class LoginBindingModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
