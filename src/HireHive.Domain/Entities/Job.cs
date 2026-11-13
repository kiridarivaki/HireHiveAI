using HireHive.Domain.Enums;

namespace HireHive.Domain.Entities;

public class Job : BaseEntity<Guid>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public JobType JobType { get; set; }
    public JobStatus Status { get; set; }
    public List<JobCriterion> Criteria { get; set; } = [];
    public Guid OwnerId { get; set; }
    #region references
    public User Owner { get; set; } = null!;
    public List<JobAssessment> Assessments { get; set; } = [];
    #endregion

    public Job() : base() { }
    public Job(string title, string description, JobType jobType, List<JobCriterion> criteria, Guid ownerId) : base()
    {
        Title = title;
        Description = description;
        JobType = jobType;
        Criteria = criteria;
        OwnerId = ownerId;
        Status = JobStatus.Open;
    }

    public void Update(string? title = null,
        string? description = null,
        JobType? jobType = null,
        List<JobCriterion>? criteria = null,
        JobStatus? status = null)
    {
        base.Update();

        Title = title ?? Title;
        Description = description ?? Description;
        JobType = jobType ?? JobType;
        Criteria = criteria ?? Criteria;
        Status = status ?? Status;
    }
}
