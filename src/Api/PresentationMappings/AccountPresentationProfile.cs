using HireHive.Api.Areas.Account.Models.BindingModels;
using HireHive.Api.Areas.Account.Models.ViewModels;
using HireHive.Application.DTOs.Account;

namespace HireHive.Api.PresentationMappings
{
    public class AccountPresentationProfile : Profile
    {
        public AccountPresentationProfile()
        {
            CreateMap<RegisterBm, RegisterDto>().ReverseMap();
            CreateMap<LoginBm, LoginDto>().ReverseMap();
            CreateMap<LoginVm, LoginDto>().ReverseMap();
            CreateMap<UserInfoVm, UserInfoDto>().ReverseMap();
        }
    }
}
