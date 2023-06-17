using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Todo.Contract.Roles;
using Todo.Contract.Users;

namespace Todo.AdminBlazor;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RoleDto, CreateUpdateRoleDto>().ReverseMap();
        CreateMap<UserDto, CreateUserDto>().ReverseMap();
        CreateMap<UserDto, UpdateUserDto>().ReverseMap();

        CreateMap<UserWithNavigationPropertiesDto, UpdateUserDto>().ReverseMap();
    }

}