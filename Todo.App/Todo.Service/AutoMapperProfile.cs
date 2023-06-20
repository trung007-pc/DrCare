using System.Security.Claims;
using AutoMapper;
using Todo.Contract.Claims;
using Todo.Contract.Roles;
using Todo.Contract.Tenants;
using Todo.Contract.Users;
using Todo.Domain.Roles;
using Todo.Domain.Tenants;
using Todo.Domain.Users;

namespace Todo.Service;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserWithNavigationProperties, UserWithNavigationPropertiesDto>().ReverseMap();
        CreateMap<CreateUserDto,User>().ReverseMap();
        CreateMap<UpdateUserDto,User>().ReverseMap();

        CreateMap<User,UserDto>().ReverseMap();

        CreateMap<Claim, ClaimDto>();
        CreateMap<CreateUpdateRoleDto, Role>();
        CreateMap<Role, RoleDto>();

        CreateMap<CreateUpdateTenantDto, Tenant>();
        CreateMap<Tenant,TenantDto>();
    }
}