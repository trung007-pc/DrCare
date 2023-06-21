using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Consts.Permissions;
using Todo.Domain.BaseEntities;
using Todo.Domain.RoleClaims;
using Todo.Domain.Roles;
using Todo.Domain.TenantClaims;
using Todo.Domain.Tenants;
using Todo.Domain.Tests;
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
    public DbSet<Test> Tests { get; set; }
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
        SeedData(builder);
        SetConstraint(builder);
        FilterGlobal(builder);


    }

    public void SetRelationShip(ModelBuilder builder)
    {
        builder.Entity<Role>()
            .HasOne<Tenant>(x => x.Tenant)
            .WithMany(x => x.Roles)
            .HasForeignKey(x => x.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        builder.Entity<TenantClaim>()
            .HasOne<Tenant>(x=>x.Tenant)
            .WithMany(x => x.Claims)
            .HasForeignKey(x => x.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict); 
    }

    public void SetConstraint(ModelBuilder builder)
    {
        builder.Entity<Tenant>().HasIndex(x => x.PhoneNumber).IsUnique(true);
        builder.Entity<Role>().HasIndex(x => x.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique(false);
    }
    

    public void FilterGlobal(ModelBuilder builder)
    {
        if (TenantId.HasValue)
        {
            builder
                .Entity<User>()
                .HasQueryFilter(x => x.TenantIds.Contains(TenantId.Value)&& !x.IsDelete);
        }
        else
        {
            builder
                .Entity<User>()
                .HasQueryFilter(x => x.TenantIds == null && !x.IsDelete);
        }
      

        builder.Entity<Role>()
            .HasQueryFilter(x => x.TenantId == TenantId);

        builder.Entity<Tenant>().HasQueryFilter(x => !x.IsDeleted);
    }
    public void SeedData(ModelBuilder builder)
    {
        builder.Entity<Role>().HasData(new Role {Id = Guid.Parse("2c5e174e-3b0e-446f-86af-483d56fd7210"), Name = "admin", NormalizedName = "admin".ToUpper() });


        //a hasher to hash the password before seeding the user to the db
        var hasher = new PasswordHasher<User>();
        
        //Seeding the User to AspNetUsers table
        builder.Entity<User>().HasData(
            new User
            {
                Id =Guid.Parse("8e445865-a24d-4543-a6c6-9443d048cdb9") ,
                FirstName = "DrCare", 
                LastName = "Nguyen",// primary key
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                IsActive = true,
                PasswordHash = hasher.HashPassword(null, "a123456")
            }
        );
        

        //Seeding the relation between our user and role to AspNetUserRoles table
        builder.Entity<UserRole>().HasData(
            new UserRole
            {
                RoleId =Guid.Parse( "2c5e174e-3b0e-446f-86af-483d56fd7210"), 
                UserId = Guid.Parse("8e445865-a24d-4543-a6c6-9443d048cdb9")
            }
        );

        builder.Entity<RoleClaim>().HasData(new RoleClaim()
        {
         Id = 1,
         ClaimType = ExtendClaimTypes.Permission,
         ClaimValue = AccessClaims.Users.Default,
         RoleId = Guid.Parse( "2c5e174e-3b0e-446f-86af-483d56fd7210")
        },
            new RoleClaim()
            {
                Id = 2,
                ClaimType = ExtendClaimTypes.Permission,
                ClaimValue = AccessClaims.Users.Authorize,
                RoleId = Guid.Parse( "2c5e174e-3b0e-446f-86af-483d56fd7210")
            },
            new RoleClaim()
            {
                Id = 3,
                ClaimType = ExtendClaimTypes.Permission,
                ClaimValue = AccessClaims.Roles.Default,
                RoleId = Guid.Parse( "2c5e174e-3b0e-446f-86af-483d56fd7210")
            }
            ,
            new RoleClaim()
            {
                Id = 4,
                ClaimType = ExtendClaimTypes.Permission,
                ClaimValue = AccessClaims.Roles.Authorize,
                RoleId = Guid.Parse("2c5e174e-3b0e-446f-86af-483d56fd7210")
            }
        
        
        
        );
    }
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<ITenant>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                    entry.Entity.TenantId = TenantId;
                    break;
            }
        }
        var result = base.SaveChanges();
        return result;
    }

  

    public override async  Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<ITenant>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                    entry.Entity.TenantId = TenantId;
                    break;
            }
        }
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}