using Domain.Enums;

namespace Application.DTOs
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public EmploymentStatus? EmploymentStatus { get; set; }
    }
}
