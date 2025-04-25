using HireHive.Application.DTOs.Resume;
using HireHive.Domain.Entities;

namespace HireHive.Infrastructure.InfrastructureMappings
{
    class ResumeInfrastructureProfile : Profile
    {
        public ResumeInfrastructureProfile()
        {
            CreateMap<Resume, UploadResumeDto>()
                .ForMember(r => r.File, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Resume, ResumeDto>().ReverseMap();
        }
    }
}
