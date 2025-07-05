using HireHive.Api.Areas.Admin.Models.BindingModels;
using HireHive.Api.Areas.Admin.Models.ViewModels;
using HireHive.Application.DTOs.Admin;

namespace HireHive.Api.PresentationMappings
{
    public class AdminPresentationMapping : Profile
    {
        public AdminPresentationMapping()
        {
            CreateMap<AssessmentParamsBm, AssessmentParamsDto>().ReverseMap();
            CreateMap<AssessmentResultDto, AssessmentResultVm>().ReverseMap();
            CreateMap<SortParamsBm, SortParamsDto>().ReverseMap();
            CreateMap<SortResultDto, SortResultVm>().ReverseMap();
        }
    }
}
