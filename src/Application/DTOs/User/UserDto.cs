using Domain.Enums;
using HireHive.Domain.Enums;

namespace HireHive.Application.DTOs.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EmploymentStatus EmploymentStatus { get; set; }
        public List<JobType>? JobTypes { get; set; }
    }
}