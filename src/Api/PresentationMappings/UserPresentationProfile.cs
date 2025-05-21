using HireHive.Api.Areas.Admin.Models.BindingModels;
using HireHive.Api.Areas.User.Models.BindingModels;
using HireHive.Api.Areas.User.Models.ViewModels;
using HireHive.Application.DTOs.User;

namespace HireHive.Api.PresentationMappings
{
    public class UserPresentationProfile : Profile
    {
        public UserPresentationProfile()
        {
            CreateMap<UserVm, UserDto>().ReverseMap();
            CreateMap<UpdateBm, UpdateDto>().ReverseMap();
            CreateMap<ListUsersBm, PaginateUsersDto>().ReverseMap();
        }
    }
}
