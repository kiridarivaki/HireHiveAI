using HireHive.Application.DTOs.Admin;
using HireHive.Application.DTOs.User;
using HireHive.Domain.Entities;

namespace HireHive.Infrastructure.InfrastructureMappings
{
    public class AdminInfrastructureProfile : Profile
    {
        public AdminInfrastructureProfile()
        {
            CreateMap<User, AssessmentResultDto>()
                .ForMember(d => d.MatchPercentage, opt => opt.Ignore())
                .ForMember(d => d.Explanation, opt => opt.Ignore());

            CreateMap<AssessedUsersDto, AssessmentResultDto>();
            CreateMap<AssessmentResultDto, SortResultDto>();
        }
    }
}
