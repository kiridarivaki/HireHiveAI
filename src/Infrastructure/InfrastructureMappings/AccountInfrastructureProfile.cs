using HireHive.Application.DTOs.Account;
using HireHive.Domain.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace HireHive.Infrastructure.InfrastructureMappings
{
    public class AccountInfrastructureProfile : Profile
    {
        public AccountInfrastructureProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<AccessTokenResponse, LoginDto>().ReverseMap();
            CreateMap<UserInfoDto, User>().ReverseMap();
        }
    }
}
