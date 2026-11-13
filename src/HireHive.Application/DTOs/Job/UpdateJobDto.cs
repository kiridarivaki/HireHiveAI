using HireHive.Domain.Enums;

namespace HireHive.Application.DTOs.Job;

public class UpdateJobDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public JobType? JobType { get; set; }
    public JobStatus? Status { get; set; }
    public List<JobCriterionDto>? Criteria { get; set; }
}
