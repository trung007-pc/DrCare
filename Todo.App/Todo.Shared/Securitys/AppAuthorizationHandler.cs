using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Todo.Core.Consts.Permissions;

namespace Todo.Core.Securitys;

public class AppAuthorizationHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var pendingRequirements = context.PendingRequirements.ToList();
        
        foreach (var requirement in pendingRequirements)
        {
            if (requirement is ClaimRequirement)
            {
                if(IsAllowed((ClaimRequirement)requirement,context.User))
                {
                    context.Succeed(requirement);
                }

            }
        }

        return Task.CompletedTask;
    }


    private bool IsAllowed(ClaimRequirement requirement,ClaimsPrincipal user)
    {
        if (user.HasClaim(x => x.Type == ExtendClaimTypes.Permission&& x.Value == requirement.Claim))
        {
            return true;
        }
        return false;
    }
}