using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Roles;
using Todo.Domain.UserRoles;
using Todo.Domain.Users;
using Todo.MongoDb.PostgreSQL;
using Todo.MongoDb.RepGenerationPatten;

namespace Todo.MongoDb.Repositorys;

public class UserRepository :GenericRepository<User,Guid>
{
    public UserRepository(TodoContext context) : base(context)
    {
        
    }

    public async Task<List<UserWithNavigationProperties>> GetListWithNavigationProperties()
    {
        var query = from user in _context.Users.Where(x=>!x.IsDelete)
            select new UserWithNavigationProperties
            {
                User = user,
                Roles = (from roleUser in _context.UserRoles
                    join role in _context.Roles on roleUser.RoleId equals role.Id
                    where roleUser.UserId == user.Id
                    select role).ToList(),
            };
        
        return await  query.ToListAsync();


    }
    public async Task<List<Role>> GetRoles(Guid userId)
    {
        var query = from userRole in _context.UserRoles.Where(x => x.UserId == userId)
            join role in _context.Roles on userRole.RoleId equals role.Id
            select role;

        return await query.ToListAsync();
    }
}

