namespace HireHive.Domain.Entities;

public class JobAssessment : BaseEntity<Guid>
{
    public Guid JobId { get; set; }
    public Guid UserId { get; set; }
    public int MatchPercentage { get; set; }
    public string Explanation { get; set; } = null!;
    #region references
    public Job Job { get; set; } = null!;
    public User User { get; set; } = null!;
    #endregion

    public JobAssessment() : base() { }
    public JobAssessment(Guid jobId, Guid userId, int matchPercentage, string explanation) : base()
    {
        JobId = jobId;
        UserId = userId;
        MatchPercentage = matchPercentage;
        Explanation = explanation;
    }

    public void Update(int? matchPercentage = null, string? explanation = null)
    {
        base.Update();

        MatchPercentage = matchPercentage ?? MatchPercentage;
        Explanation = explanation ?? Explanation;
    }
}
