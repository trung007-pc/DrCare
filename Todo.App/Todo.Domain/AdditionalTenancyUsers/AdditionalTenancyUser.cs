using Todo.Domain.Tenants;
using Todo.Domain.Users;

namespace Todo.Domain.AdditionalTenancyUsers;

public class AdditionalTenancyUser
{
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public User User { get; set; }
    public Tenant Tenant { get; set; }
}