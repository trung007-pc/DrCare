using Microsoft.AspNetCore.Identity;

namespace Todo.Domain.UserClaims;

public class UserClaim  : IdentityUserClaim<Guid>
{
}