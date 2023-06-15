using System.Security.Claims;
using Todo.Contract.Claims;
using Todo.Contract.RoleClaims;

namespace Todo.Contract.Roles;

public interface IRoleService 
{
    Task<List<RoleDto>> GetListAsync();
    Task<RoleDto> CreateAsync(CreateUpdateRoleDto input);
    Task<RoleDto> UpdateAsync(CreateUpdateRoleDto input,Guid id);
    Task DeleteAsync(Guid id);
    
    Task CreateWithClaimsAsync(CreateUpdateRoleDto input);
    Task UpdateWithClaimsAsync(CreateUpdateRoleDto input,Guid id);
    Task UpdateClaimsToRole(Guid id, List<CreateUpdateClaimDto> claims);
    Task<List<ClaimDto>> GetClaims(Guid roleId);
}