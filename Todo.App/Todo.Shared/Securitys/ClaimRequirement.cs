using Microsoft.AspNetCore.Authorization;

namespace Todo.Core.Securitys;

public class ClaimRequirement : IAuthorizationRequirement
{
    public string Claim { get; private set; }

    public ClaimRequirement(string claim)
    {
        Claim = claim;
    }
}