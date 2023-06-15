using Todo.Core.DependencyRegistrationTypes;
using Todo.Domain.RoleClaims;
using Todo.Domain.Users;
using Todo.MongoDb.PostgreSQL;

namespace Todo.MongoDb.Repositorys;

public class UnitOfWork : ITransientDependency
{
    private TodoContext _context { get; set; }
    private UserRepository _userRepository { get; set; }
    private RoleRepository _roleRepository { get; set; }
    private UserRoleRepository _userRolesRepository { get; set; }
    private RoleClaimRepository _roleClaimsRepository { get; set; }
    private UserClaimRepository _userClaimsRepository { get; set; }
    private TenantRepository _tenantRepository { get; set; }


    public UnitOfWork(TodoContext context)
    {
        _context = context;
    }

    public UserRepository UserRepository
    {
        get
        {
            return this._userRepository ?? new UserRepository(_context);
        }
    }
    
    public RoleRepository RoleRepository
    {
        get
        {
            return this._roleRepository ?? new RoleRepository(_context);
        }
    }
    
    public UserRoleRepository UserRoleRepository
    {
        get
        {
            return this._userRolesRepository ?? new UserRoleRepository(_context);
        }
    }
    
    public RoleClaimRepository RoleClaimRepository
    {
        get
        {
            return this._roleClaimsRepository ?? new RoleClaimRepository(_context);
        }
    }
    
    public UserClaimRepository UserClaimRepository
    {
        get
        {
            return this._userClaimsRepository ?? new UserClaimRepository(_context);
        }
    }
    public TenantRepository TenantRepository
    {
        get
        {
            return this._tenantRepository ?? new TenantRepository(_context);
        }
    }
    public void SaveChange()
    {
        _context.SaveChanges();
    }
    
    
}