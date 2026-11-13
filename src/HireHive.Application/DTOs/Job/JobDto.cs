using HireHive.Domain.Enums;

namespace HireHive.Application.DTOs.Job;

public class JobDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public JobType JobType { get; set; }
    public JobStatus Status { get; set; }
    public List<JobCriterionDto> Criteria { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
