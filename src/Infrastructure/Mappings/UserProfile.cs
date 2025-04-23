using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Identity;

namespace Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, User>().ReverseMap();
            CreateMap<RegisterUserDto, User>().ReverseMap();
        }
    }
}
