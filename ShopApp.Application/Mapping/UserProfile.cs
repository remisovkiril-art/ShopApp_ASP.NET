using AutoMapper;
using ShopApplication.DTOs.UserDTOs;
using ShopDomain.Models;

namespace ShopApplication.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserCreateDTO, User>();
        CreateMap<User, UserReadDTO>();
        CreateMap<User, UserLoginDTO>();
    }
}
