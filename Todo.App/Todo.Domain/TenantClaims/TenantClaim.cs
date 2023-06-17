using Todo.Domain.BaseEntities;

namespace Todo.Domain.TenantClaims;

public class TenantClaim : Entity
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
}