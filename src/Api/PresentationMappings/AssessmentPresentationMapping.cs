using HireHive.Api.Areas.Admin.Models.BindingModels;
using HireHive.Api.Areas.Admin.Models.ViewModels;
using HireHive.Application.DTOs.Admin;

namespace HireHive.Api.PresentationMappings
{
    public class AssessmentPresentationMapping : Profile
    {
        public AssessmentPresentationMapping()
        {
            CreateMap<AssessmentBm, AssessmentDto>().ReverseMap();
            CreateMap<AssessmentDto, AssessmentVm>().ReverseMap();
        }
    }
}
