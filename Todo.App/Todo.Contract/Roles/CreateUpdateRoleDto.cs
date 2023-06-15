using Todo.Contract.Claims;
using Todo.Contract.RoleClaims;

namespace Todo.Contract.Roles;

public class CreateUpdateRoleDto
{ 
        public string Name { get; set; }
        public string? Code { get; set; }
        public List<CreateUpdateClaimDto> Claims { get; set; } = new List<CreateUpdateClaimDto>();
}