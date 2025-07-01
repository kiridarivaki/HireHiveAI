using HireHive.Application.DTOs.Admin;
using HireHive.Application.DTOs.User;

namespace HireHive.Infrastructure.InfrastructureMappings
{
    public class AdminInfrastructureProfile : Profile
    {
        public AdminInfrastructureProfile()
        {
            CreateMap<AssessedUsersDto, AssessmentResultDto>();
            CreateMap<AssessmentResultDto, SortResultDto>();
        }
    }
}
