using Todo.Domain.BaseEntities;
using Todo.Domain.Tenants;

namespace Todo.Domain.TenantClaims;

public class TenantClaim : Entity
{
    public Guid TenantId { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
    
    public Tenant Tenant { get; set; }
}