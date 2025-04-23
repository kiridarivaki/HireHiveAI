using Domain.Enums;

namespace Api.Areas.User.Models
{
    public class UpdateUserBindingModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public EmploymentStatus? EmploymentStatus { get; set; }
    }
}
