using JetBrains.Annotations;
using Todo.Domain.Tenants;
using Todo.Domain.Tests;
using Todo.MongoDb.PostgreSQL;
using Todo.MongoDb.RepGenerationPatten;

namespace Todo.MongoDb.Repositorys;

public class TestRepository :GenericRepository<Test,Guid>
{
    public TestRepository([NotNull] TodoContext context) : base(context)
    {
    }
}