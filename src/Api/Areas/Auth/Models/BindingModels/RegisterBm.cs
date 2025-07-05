using Domain.Enums;
using HireHive.Domain.Enums;

namespace HireHive.Api.Areas.Auth.Models.BindingModels;

public class RegisterBm
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public EmploymentStatus EmploymentStatus { get; set; }
    public List<JobType> JobTypes { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}
