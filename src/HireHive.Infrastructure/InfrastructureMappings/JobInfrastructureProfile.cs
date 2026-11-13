using HireHive.Application.DTOs.Job;
using HireHive.Domain.Entities;

namespace HireHive.Infrastructure.InfrastructureMappings;

public class JobInfrastructureProfile : Profile
{
    public JobInfrastructureProfile()
    {
        CreateMap<JobCriterion, JobCriterionDto>().ReverseMap();

        CreateMap<Job, JobDto>()
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => s.Audit.CreatedAt));

        CreateMap<JobAssessment, JobAssessmentDto>()
            .ForMember(d => d.AssessedAt, opt => opt.MapFrom(s => s.Audit.UpdatedAt));
    }
}
