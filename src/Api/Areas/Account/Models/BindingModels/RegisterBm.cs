using Domain.Enums;

namespace HireHive.Api.Areas.Account.Models.BindingModels;

public class RegisterBm
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public EmploymentStatus EmploymentStatus { get; set; }
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}
