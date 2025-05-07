using Domain.Enums;

namespace HireHive.Api.Areas.User.Models.ViewModels
{
    public class UserVm
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EmploymentStatus EmploymentStatus { get; set; }
    }
}
