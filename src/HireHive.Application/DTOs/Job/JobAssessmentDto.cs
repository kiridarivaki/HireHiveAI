namespace HireHive.Application.DTOs.Job;

public class JobAssessmentDto
{
    public Guid UserId { get; set; }
    public int MatchPercentage { get; set; }
    public string Explanation { get; set; } = null!;
    public DateTime AssessedAt { get; set; }
}
