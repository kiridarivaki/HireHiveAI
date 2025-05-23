using HireHive.Application.DTOs.Admin;
using HireHive.Application.DTOs.User;

namespace HireHive.Infrastructure.InfrastructureMappings
{
    public class AssessmentInfrastructureProfile : Profile
    {
        public AssessmentInfrastructureProfile()
        {
            CreateMap<UsersDto, AssessmentResultDto>();
        }
    }
}
