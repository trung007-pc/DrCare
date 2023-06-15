namespace Todo.Contract.Claims;

public class UserClaimsDto
{
    public List<ClaimDto> UserClaims { get; set; } = new List<ClaimDto>();
    public List<ClaimDto> RoleClaims { get; set; } = new List<ClaimDto>();
}