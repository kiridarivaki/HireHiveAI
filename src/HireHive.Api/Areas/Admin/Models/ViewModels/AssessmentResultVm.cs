namespace HireHive.Api.Areas.Admin.Models.ViewModels
{
    public class AssessmentResultVm
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int MatchPercentage { get; set; }
        public string Explanation { get; set; } = null!;
    }
}
