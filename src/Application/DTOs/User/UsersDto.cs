using Domain.Enums;

namespace HireHive.Application.DTOs.User
{
    public class UsersDto
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EmploymentStatus EmploymentStatus { get; set; }
    }
}
