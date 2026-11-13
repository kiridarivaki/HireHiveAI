namespace HireHive.Api.Areas.Job.Models.ViewModels;

public class JobAssessmentVm
{
    public Guid UserId { get; set; }
    public int MatchPercentage { get; set; }
    public string Explanation { get; set; } = null!;
    public DateTime AssessedAt { get; set; }
}
