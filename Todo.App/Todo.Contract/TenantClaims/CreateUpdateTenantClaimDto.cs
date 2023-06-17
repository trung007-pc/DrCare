namespace Todo.Contract.TenantClaims;

public class CreateUpdateTenantClaimDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
}