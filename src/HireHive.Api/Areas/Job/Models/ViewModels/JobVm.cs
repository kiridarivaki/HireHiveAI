using HireHive.Domain.Enums;

namespace HireHive.Api.Areas.Job.Models.ViewModels;

public class JobVm
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public JobType JobType { get; set; }
    public JobStatus Status { get; set; }
    public List<JobCriterionVm> Criteria { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
