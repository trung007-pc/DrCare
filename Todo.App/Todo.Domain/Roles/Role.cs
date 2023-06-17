using Microsoft.AspNetCore.Identity;
using Todo.Domain.BaseEntities;
using Todo.Domain.Tenants;

namespace Todo.Domain.Roles;

public class Role :  IdentityRole<Guid>,ITenant
{
    public string Code { get; set; }
    public Guid? TenantId { get; set; }
    public Tenant Tenant { get; set; }
}