using HireHive.Domain.Enums;

namespace HireHive.Api.Areas.Job.Models.BindingModels;

public class CreateJobBm
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public JobType JobType { get; set; }
    public List<JobCriterionBm> Criteria { get; set; } = [];
}
