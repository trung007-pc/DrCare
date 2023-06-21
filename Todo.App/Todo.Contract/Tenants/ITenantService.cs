using Todo.Contract.Claims;

namespace Todo.Contract.Tenants;

public interface ITenantService
{
    Task<List<TenantDto>> GetListAsync();
    Task<TenantDto> CreateAsync(CreateUpdateTenantDto input);
    Task<TenantDto> UpdateAsync(CreateUpdateTenantDto input,Guid id);
    Task DeleteAsync(Guid id);
    Task UpdateClaims(Guid id, List<CreateUpdateClaimDto> claims);
    Task<List<ClaimDto>> GetClaims(Guid tenantId);
}