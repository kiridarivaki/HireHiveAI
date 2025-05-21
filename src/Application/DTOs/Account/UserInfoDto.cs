using Domain.Enums;

namespace HireHive.Application.DTOs.Account
{
    public class UserInfoDto
    {
        public List<string> Roles { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EmploymentStatus EmploymentStatus { get; set; }
    }
}
