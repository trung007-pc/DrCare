using Todo.Domain.UserClaims;
using Todo.MongoDb.PostgreSQL;
using Todo.MongoDb.RepGenerationPatten;

namespace Todo.MongoDb.Repositorys;

public class UserClaimRepository  : GenericRepository<UserClaim,Guid>
{

    public UserClaimRepository(TodoContext context) : base(context)
    {
    }
    
}