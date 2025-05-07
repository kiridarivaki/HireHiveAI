using HireHive.Api.Areas.Resume.Models.BindingModels;
using HireHive.Api.Areas.Resume.Models.ViewModels;
using HireHive.Application.DTOs.Resume;

namespace HireHive.Api.PresentationMappings
{
    public class ResumePresentationProfile : Profile
    {
        public ResumePresentationProfile()
        {
            CreateMap<UploadResumeBm, UploadResumeDto>().ReverseMap();
            CreateMap<UploadResumeVm, UploadResumeDto>().ReverseMap();
            CreateMap<UpdateResumeBm, UpdateResumeDto>().ReverseMap();
            CreateMap<ResumeVm, ResumeDto>().ReverseMap();
        }
    }
}
