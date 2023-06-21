using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Consts.ErrorCodes;
using Todo.Core.DependencyRegistrationTypes;
using Todo.Domain.Roles;
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
        var userRepo = _unitOfWork.UserRepository;
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user,"admin");


        if (await userRepo.Entity.IgnoreQueryFilters().AnyAsync(x => x.UserName == user.UserName))
        {
            throw new GlobalException(L[TenantErrorCode.PhoneNumberAlreadyExist], HttpStatusCode.BadRequest);
        }
        userRepo.Entity.Add(user);
        
        await _unitOfWork.UserRepository.Entity.AddAsync(user); 
        await _unitOfWork
            .RoleRepository.
             Entity.AddAsync(role);
       await  _unitOfWork.UserRoleRepository.Entity.AddAsync(new UserRole()
        {
            RoleId = role.Id,
            UserId = user.Id
        });
    }
}