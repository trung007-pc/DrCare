using Microsoft.AspNetCore.Identity;
using Todo.Core.DependencyRegistrationTypes;
using Todo.Domain.Roles;
using Todo.Domain.Tenants;
using Todo.Domain.UserRoles;
using Todo.Domain.Users;
using Todo.Localziration;
using Todo.MongoDb.Repositorys;

namespace Todo.Service.Users;

public class SharedUserService : IScopeDependency
{
    private UserManager<User> _userManager;
    private UnitOfWork _unitOfWork { get; set; }
    public SharedUserService(UserManager<User> userManager,Localizer localizer)
    {
        _userManager = userManager;
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
            TenantId = tenant.Id
        };
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user,"admin");
        _unitOfWork.UserRepository.Entity.Add(user);

        
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