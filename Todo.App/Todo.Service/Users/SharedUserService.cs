using System.Net;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todo.Contract.Claims;
using Todo.Core.Consts.ErrorCodes;
using Todo.Core.Consts.Permissions;
using Todo.Core.DependencyRegistrationTypes;
using Todo.Domain.RoleClaims;
using Todo.Domain.Roles;
using Todo.Domain.TenantClaims;
using Todo.Domain.Tenants;
using Todo.Domain.UserRoles;
using Todo.Domain.Users;
using Todo.Localziration;
using Todo.MongoDb.Repositorys;
using Todo.Service.Exceptions;

namespace Todo.Service.Users;

public class SharedUserService : IScopeDependency
{
    private UserManager<User> _userManager;
    private UnitOfWork _unitOfWork { get; set; }
    private Localizer L { get; set; }
    public SharedUserService(UserManager<User> userManager,Localizer l)
    {
        _userManager = userManager;
        L = l;
    }

    public void InjectUnitOfWork(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    

    public async Task CreateUserAndRoleDefaultToTenant(Tenant tenant)
    {

        var role = new Role()
        {
            Id = Guid.NewGuid(),
            Name = "admin",
            NormalizedName = "ADMIN",
            TenantId = tenant.Id,
        };
        var user = new User()
        {
            FirstName = "DrCare",
            LastName = "Nguyen",
            UserName = tenant.PhoneNumber,
            NormalizedUserName = tenant.PhoneNumber.ToUpper(),
            PhoneNumber = tenant.PhoneNumber,
            TenantIds = new List<Guid>(){tenant.Id}
        };
        var claimDefault = GetClaimDefault();
        var roleClaims = new List<RoleClaim>();
        var tenantClaims = new List<TenantClaim>();
        foreach (var item in claimDefault)
        {
            roleClaims.Add(new RoleClaim()
            {
                ClaimType = item.Type,
                ClaimValue = item.Value,
                RoleId = role.Id
            });
            tenantClaims.Add(new TenantClaim()
            {
                ClaimType = item.Type,
                ClaimValue = item.Value,
                TenantId = tenant.Id,
            });
        }

        
        var userRepo = _unitOfWork.UserRepository;
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user,"admin");
        
          
        if (await userRepo.Entity.IgnoreQueryFilters().AnyAsync(x => x.UserName == user.UserName))
        {
            throw new GlobalException(L[TenantErrorCode.PhoneNumberAlreadyExist], HttpStatusCode.BadRequest);
        }
        
        userRepo.Entity.Add(user);
        
        await _unitOfWork
            .RoleRepository.
             Entity.AddAsync(role);
       await  _unitOfWork.UserRoleRepository.Entity.AddAsync(new UserRole()
        {
            RoleId = role.Id,
            UserId = user.Id
        });
       _unitOfWork.TenantClaimRepository
           .Entity.
           AddRange(tenantClaims);
       _unitOfWork.RoleClaimRepository
           .Entity
           .AddRange(roleClaims);
    }

    private List<ClaimDto> GetClaimDefault()
    {
        return new List<ClaimDto>()
        {
            new ClaimDto()
            {
                Type = ExtendClaimTypes.Permission,
                Value = AccessClaims.Roles.Default
            },
            new ClaimDto()
            {
                Type = ExtendClaimTypes.Permission,
                Value = AccessClaims.Roles.Create
            },
            new ClaimDto()
            {
                Type = ExtendClaimTypes.Permission,
                Value = AccessClaims.Roles.Edit
            },
            new ClaimDto()
            {
                Type = ExtendClaimTypes.Permission,
                Value = AccessClaims.Roles.Delete
            },
            new ClaimDto()
            {
                Type = ExtendClaimTypes.Permission,
                Value = AccessClaims.Roles.Authorize
            },
            new ClaimDto()
            {
                Type = ExtendClaimTypes.Permission,
                Value = AccessClaims.Users.Default
            },
            new ClaimDto()
            {
                Type = ExtendClaimTypes.Permission,
                Value = AccessClaims.Users.Create
            },
            new ClaimDto()
            {
                Type = ExtendClaimTypes.Permission,
                Value = AccessClaims.Users.Edit
            },
            new ClaimDto()
            {
                Type = ExtendClaimTypes.Permission,
                Value = AccessClaims.Users.Delete
            }
            ,   new ClaimDto()
            {
                Type = ExtendClaimTypes.Permission,
                Value = AccessClaims.Users.Authorize
            }
        };
    }
}