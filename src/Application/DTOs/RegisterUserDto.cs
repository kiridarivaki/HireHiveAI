using Domain.Enums;

namespace Application.DTOs
{
    public class RegisterUserDto
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EmploymentStatus EmploymentStatus { get; set; }
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
