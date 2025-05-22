namespace HireHive.Api.Areas.Admin.Models.ViewModels
{
    public class AssessmentVm
    {
        public List<string>? MatchPercentages { get; set; } = null!;
        public List<Guid>? UserIds { get; set; } = null!;
    }
}
