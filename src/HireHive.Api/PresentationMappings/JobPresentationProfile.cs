using HireHive.Api.Areas.Job.Models.BindingModels;
using HireHive.Api.Areas.Job.Models.ViewModels;
using HireHive.Application.DTOs.Job;

namespace HireHive.Api.PresentationMappings;

public class JobPresentationProfile : Profile
{
    public JobPresentationProfile()
    {
        CreateMap<JobCriterionBm, JobCriterionDto>();
        CreateMap<CreateJobBm, CreateJobDto>();
        CreateMap<UpdateJobBm, UpdateJobDto>();

        CreateMap<JobCriterionDto, JobCriterionVm>();
        CreateMap<JobDto, JobVm>();
        CreateMap<JobAssessmentDto, JobAssessmentVm>();
    }
}
