using Domain.Enums;
using HireHive.Domain.Enums;

namespace HireHive.Application.DTOs.User
{
    public class UpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public EmploymentStatus? EmploymentStatus { get; set; }
        public List<JobType>? JobTypes { get; set; }
    }
}
