using Domain.Enums;

namespace HireHive.Api.Areas.Admin.Models.ViewModels
{
    public class SortResultVm
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public EmploymentStatus EmploymentStatus { get; set; }
        public int MatchPercentage { get; set; }
    }
}
