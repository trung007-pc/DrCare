using Todo.Contract.Claims;

namespace Todo.Contract.TenantClaims;

public interface ITenantClaimService
{
    Task<List<TenantClaimDto>> GetListAsync();
    Task UpdateClaims(List<ClaimDto> inputs);
}