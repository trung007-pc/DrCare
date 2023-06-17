using JetBrains.Annotations;
using Todo.Domain.TenantClaims;
using Todo.MongoDb.PostgreSQL;
using Todo.MongoDb.RepGenerationPatten;

namespace Todo.MongoDb.Repositorys;

public class TenantClaimRepository :GenericRepository<TenantClaim,Guid>
{
    public TenantClaimRepository([NotNull] TodoContext context) : base(context)
    {
    }
}