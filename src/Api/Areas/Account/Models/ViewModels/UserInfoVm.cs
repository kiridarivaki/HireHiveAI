using Domain.Enums;

namespace HireHive.Api.Areas.Account.Models.ViewModels
{
    public class UserInfoVm
    {
        public List<string> Roles { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EmploymentStatus EmploymentStatus { get; set; }
    }
}
