using AutoMapper;
using HireHive.Api.Areas.Account.Models;
using HireHive.Api.Areas.User.Models;
using HireHive.Application.DTOs.Account;
using HireHive.Application.DTOs.User;

namespace HireHive.Api.PresentationMappings
{
    public class UserPresentationProfile : Profile
    {
        public UserPresentationProfile()
        {
            CreateMap<RegisterBm, RegisterDto>().ReverseMap();
            CreateMap<UpdateBm, UpdateDto>().ReverseMap();
            CreateMap<LoginBm, LoginDto>().ReverseMap();
        }
    }
}
