using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.RoleClaims;
using Todo.Domain.Roles;
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

    public TodoContext(DbContextOptions<TodoContext> options) : base(options)  
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
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
        
        
      

    }


}