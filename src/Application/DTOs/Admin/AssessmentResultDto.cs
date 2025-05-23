using Domain.Enums;

namespace HireHive.Application.DTOs.Admin
{
    public class AssessmentResultDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EmploymentStatus EmploymentStatus { get; set; }
        public int MatchPercentage { get; set; }
    }
}
