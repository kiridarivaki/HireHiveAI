using HireHive.Application.DTOs.User;
using HireHive.Domain.Entities;

namespace HireHive.Infrastructure.InfrastructureMappings;

public class UserInfastructureProfile : Profile
{
    public UserInfastructureProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, AssessedUsersDto>();
        CreateMap<UpdateDto, User>();
    }
}
