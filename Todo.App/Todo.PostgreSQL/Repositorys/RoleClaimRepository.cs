using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.RoleClaims;
using Todo.MongoDb.PostgreSQL;
using Todo.MongoDb.RepGenerationPatten;

namespace Todo.MongoDb.Repositorys;

public class RoleClaimRepository :GenericRepository<RoleClaim,Guid>
{
    public RoleClaimRepository(TodoContext context) : base(context)
    {
    }


    public async Task<List<Claim>> GetClaimsByRoleIds(List<Guid> ids)
    {
        var result = await _context.RoleClaims
            .Where(x => ids.Contains(x.RoleId))
            .ToListAsync();
        return result.Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList();
    }
}