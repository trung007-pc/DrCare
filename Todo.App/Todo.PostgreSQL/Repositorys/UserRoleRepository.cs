using Todo.Domain.UserRoles;
using Todo.MongoDb.PostgreSQL;
using Todo.MongoDb.RepGenerationPatten;

namespace Todo.MongoDb.Repositorys;

public class UserRoleRepository :GenericRepository<UserRole,Guid>
{
    public UserRoleRepository(TodoContext context) : base(context)
    {
    }
}