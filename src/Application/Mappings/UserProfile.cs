using Application.DTOs;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Domain.Entities;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<DomainUser, UserDto>().ReverseMap();
    }
}

