using Application.DTOs;
using Domain.Entities;

namespace Application.Mappings;

public class UserDtoProfile : Profile
{
    public UserDtoProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<RegisterUserDto, User>();
        CreateMap<UpdateUserDto, User>();
    }
}
