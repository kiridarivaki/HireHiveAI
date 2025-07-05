using HireHive.Api.Areas.Auth.Models.BindingModels;
using HireHive.Api.Areas.Auth.Models.ViewModels;
using HireHive.Application.DTOs.Auth;

namespace HireHive.Api.PresentationMappings
{
    public class AuthPresentationProfile : Profile
    {
        public AuthPresentationProfile()
        {
            CreateMap<RegisterBm, RegisterDto>().ReverseMap();
            CreateMap<LoginBm, LoginDto>().ReverseMap();
            CreateMap<RefreshBm, RefreshDto>().ReverseMap();
            CreateMap<AuthenticatedUserVm, AuthenticatedUserDto>().ReverseMap();
            CreateMap<UserInfoVm, UserInfoDto>().ReverseMap();
        }
    }
}
