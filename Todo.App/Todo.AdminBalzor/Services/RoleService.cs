﻿using System.Security.Claims;
using Blazored.LocalStorage;
using Todo.AdminBlazor.Network;
using Todo.Contract.Claims;
using Todo.Contract.Roles;
using Todo.Core.DependencyRegistrationTypes;

namespace Todo.AdminBlazor.Services;

public class RoleService :IRoleService,ITransientDependency
{

    private HttpClient _client;
    public RoleService(ClientSetter setter)
    {
        _client = setter.GetClient("DefaultClient");
    }
    

    public async Task<List<RoleDto>> GetListAsync()
    {
        return await _client.GetAPIAsync<List<RoleDto>>("role");
    }

    public async Task<RoleDto> CreateAsync(CreateUpdateRoleDto input)
    {
        return await _client.PostAPIAsync<RoleDto>("role",input);

    }

    public async Task<RoleDto> UpdateAsync(CreateUpdateRoleDto input, Guid id)
    {
        return await _client.PutAPIAsync<RoleDto>($"role/{id}" , input);

    }

    public async Task DeleteAsync(Guid id)
    { 
        await _client.DeleteAPIAsync<Task>($"role/{id}");
    }

    public Task CreateWithClaimsAsync(CreateUpdateRoleDto input)
    {
        throw new NotImplementedException();
    }

    public Task UpdateWithClaimsAsync(CreateUpdateRoleDto input, Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateClaimsToRole(Guid id, List<CreateUpdateClaimDto> claims)
    {
         await _client.PutAPIAsync<Task>($"role/update-claims-to-role/{id}" , claims);
    }

    public async Task<List<ClaimDto>> GetClaims(Guid roleId)
    {
      return   await _client.GetAPIAsync<List<ClaimDto>>($"role/get-claims/{roleId.ToString()}" );
    }
}