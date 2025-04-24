using Domain.Enums;

namespace HireHive.Api.Areas.User.Models
{
    public class UpdateBm
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public EmploymentStatus? EmploymentStatus { get; set; }
    }
}
