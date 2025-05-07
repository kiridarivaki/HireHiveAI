using HireHive.Application.DTOs.Account;
using HireHive.Application.DTOs.User;
using HireHive.Domain.Entities;

namespace HireHive.Infrastructure.InfrastructureMappings;

public class UserInfastructureProfile : Profile
{
    public UserInfastructureProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<RegisterDto, User>();
        CreateMap<UpdateDto, User>();
    }
}
