using HireHive.Application.DTOs.Auth;
using HireHive.Domain.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace HireHive.Infrastructure.InfrastructureMappings
{
    public class AuthInfrastructureProfile : Profile
    {
        public AuthInfrastructureProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<AccessTokenResponse, LoginDto>().ReverseMap();
            CreateMap<UserInfoDto, User>().ReverseMap();
        }
    }
}
