using HireHive.Api.Areas.Admin.Models.BindingModels;
using HireHive.Api.Areas.Admin.Models.ViewModels;
using HireHive.Application.DTOs.Admin;

namespace HireHive.Api.PresentationMappings
{
    public class AssessmentPresentationMapping : Profile
    {
        public AssessmentPresentationMapping()
        {
            CreateMap<AssessmentParamsBm, AssessmentParamsDto>().ReverseMap();
            CreateMap<AssessmentResultDto, AssessmentResultVm>().ReverseMap();
        }
    }
}
