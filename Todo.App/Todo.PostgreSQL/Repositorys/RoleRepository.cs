using Microsoft.EntityFrameworkCore;
using Todo.Domain.Roles;
using Todo.MongoDb.PostgreSQL;
using Todo.MongoDb.RepGenerationPatten;

namespace Todo.MongoDb.Repositorys;

public class RoleRepository :GenericRepository<Role,Guid>
{
    public RoleRepository(TodoContext context) : base(context)
    {
    }

    public async Task<List<Guid>> GetRoleIds(Guid userId)
    {
       var userRoles = _context
            .UserRoles
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x=>x.RoleId);

       return await userRoles.ToListAsync();
    }
}