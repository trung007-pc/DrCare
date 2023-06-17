using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Consts.Permissions;
using Todo.Domain.RoleClaims;
using Todo.Domain.Roles;
using Todo.Domain.TenantClaims;
using Todo.Domain.Tenants;
using Todo.Domain.UserClaims;
using Todo.Domain.UserLogins;
using Todo.Domain.UserRoles;
using Todo.Domain.Users;
using Todo.Domain.UserTokens;

namespace Todo.MongoDb.PostgreSQL;

public class TodoContext :  IdentityDbContext<User,Role,Guid,UserClaim,UserRole,UserLogin,RoleClaim,UserToken>
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleClaim> RoleClaims { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantClaim> TenantClaims { get; set; }
    private  Guid? TenantId { get; set; }

    public TodoContext(DbContextOptions<TodoContext> options,TenantContext tenantContext) : base(options)  
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        TenantId = tenantContext.TenantId;
    }  

    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        //default ondelete cascade
        //get rid of prefix Asp
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
        
        SetRelationShip(builder);
        FilterGlobal(builder);


    }

    public void SetRelationShip(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasOne<Tenant>(x => x.Tenant)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Role>()
            .HasOne<Tenant>(x => x.Tenant)
            .WithMany(x => x.Roles)
            .HasForeignKey(x => x.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public void FilterGlobal(ModelBuilder builder)
    {
        builder
            .Entity<User>()
            .HasQueryFilter(x => x.TenantId == TenantId && !x.IsDelete);

        builder.Entity<Role>()
            .HasQueryFilter(x => x.TenantId == TenantId);
        builder.Entity<TenantClaim>()
            .HasQueryFilter(x => x.TenantId == TenantId);
    }


}