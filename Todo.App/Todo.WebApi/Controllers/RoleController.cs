using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Contract.Claims;
using Todo.Contract.Roles;
using Todo.Service.Roles;

namespace Todo.App.Controllers;

[ApiController]
[Route("api/role/")]
public class RoleController : IRoleService
{
    private RoleService _roleService;

    public RoleController(RoleService roleService)
    {
        _roleService = roleService;
    }
    
    [HttpGet]
    public async Task<List<RoleDto>> GetListAsync()
    {
        return await _roleService.GetListAsync();
    }

    [HttpPost]
    public async Task<RoleDto> CreateAsync(CreateUpdateRoleDto input)
    {
        
        return await _roleService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<RoleDto> UpdateAsync(CreateUpdateRoleDto input, Guid id)
    { 
        return await _roleService.UpdateAsync(input,id);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task DeleteAsync(Guid id)
    {
         await _roleService.DeleteAsync(id);
    }
    
    [HttpPost]
    [Route("create-with-claims")]
    public async Task CreateWithClaimsAsync(CreateUpdateRoleDto input)
    {
        await _roleService.CreateWithClaimsAsync(input);
    }
    
    [HttpPut]
    [Route("update-with-claims")]
    public async Task UpdateWithClaimsAsync(CreateUpdateRoleDto input, Guid id)
    {
        await _roleService.UpdateWithClaimsAsync(input,id);
    }
    
    [HttpPut]
    [Route("update-claims-to-role/{id}")]
    public async Task UpdateClaimsToRole(Guid id, List<CreateUpdateClaimDto> claims)
    {
        await _roleService.UpdateClaimsToRole(id,claims);
    }

    [HttpGet]
    [Route("get-claims/{roleId}")]
    public async Task<List<ClaimDto>> GetClaims(Guid roleId)
    {
       return  await _roleService.GetClaims(roleId);
    }
}