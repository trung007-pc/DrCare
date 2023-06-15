using Microsoft.AspNetCore.Identity;

namespace Todo.Domain.UserClaims;

public class UserClaim  : IdentityUserClaim<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
}