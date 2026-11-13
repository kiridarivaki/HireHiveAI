using HireHive.Domain.Enums;

namespace HireHive.Api.Areas.Job.Models.BindingModels;

public class UpdateJobBm
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public JobType? JobType { get; set; }
    public JobStatus? Status { get; set; }
    public List<JobCriterionBm>? Criteria { get; set; }
}
