using Api.Areas.User.Models;
using Application.DTOs;
using AutoMapper;

namespace Api.Areas.User.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserBindingModel, UserDto>().ReverseMap();
        }
    }
}
