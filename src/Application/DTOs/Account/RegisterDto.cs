using Domain.Enums;
using HireHive.Domain.Enums;

namespace HireHive.Application.DTOs.Account
{
    public class RegisterDto
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EmploymentStatus EmploymentStatus { get; set; }
        public List<JobType>? JobTypes { get; set; }
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
