using JetBrains.Annotations;
using Todo.Domain.Tenants;
using Todo.MongoDb.PostgreSQL;
using Todo.MongoDb.RepGenerationPatten;

namespace Todo.MongoDb.Repositorys;

public class TenantRepository :GenericRepository<Tenant,Guid>
{
    public TenantRepository([NotNull] TodoContext context) : base(context)
    {
    }
}