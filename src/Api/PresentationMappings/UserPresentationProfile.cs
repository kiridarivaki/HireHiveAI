using HireHive.Api.Areas.Account.Models.BindingModels;
using HireHive.Api.Areas.Account.Models.ViewModels;
using HireHive.Api.Areas.User.Models.BindingModels;
using HireHive.Api.Areas.User.Models.ViewModels;
using HireHive.Application.DTOs.Account;
using HireHive.Application.DTOs.User;

namespace HireHive.Api.PresentationMappings
{
    public class UserPresentationProfile : Profile
    {
        public UserPresentationProfile()
        {
            CreateMap<UserVm, UserDto>().ReverseMap();
            CreateMap<RegisterBm, RegisterDto>().ReverseMap();
            CreateMap<UpdateBm, UpdateDto>().ReverseMap();
            CreateMap<LoginBm, LoginDto>().ReverseMap();
            CreateMap<LoginVm, LoginDto>().ReverseMap();
        }
    }
}
