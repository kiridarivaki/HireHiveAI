using HireHive.Domain.Enums;

namespace HireHive.Application.DTOs.Job;

public class CreateJobDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public JobType JobType { get; set; }
    public List<JobCriterionDto> Criteria { get; set; } = [];
}
