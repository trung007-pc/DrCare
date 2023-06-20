using Blazored.LocalStorage;
using Todo.AdminBlazor.Network;
using Todo.Contract.Claims;
using Todo.Contract.Tenants;
using Todo.Contract.Tenants;
using Todo.Core.DependencyRegistrationTypes;

namespace Todo.AdminBlazor.Services;

public class TenantService :ITenantService,ITransientDependency
{
    
    private HttpClient _client;
    public TenantService(ClientSetter setter)
    {
        _client = setter.GetClient("DefaultClient");
    }
    public async Task<List<TenantDto>> GetListAsync()
    {
        return await _client.GetAPIAsync<List<TenantDto>>("tenant");
    }

    public async Task<TenantDto> CreateAsync(CreateUpdateTenantDto input)
    {
        return await _client.PostAPIAsync<TenantDto>("tenant",input);

    }

    public async Task<TenantDto> UpdateAsync(CreateUpdateTenantDto input, Guid id)
    {
        return await _client.PutAPIAsync<TenantDto>($"tenant/{id}" , input);

    }

    public async Task DeleteAsync(Guid id)
    { 
        await _client.DeleteAPIAsync<Task>($"tenant/{id}");
    }

    public async Task UpdateClaims(Guid id, List<CreateUpdateClaimDto> claims)
    {
         await _client.PutAPIAsync<Task>($"tenant/update-claims/{id}" , claims);
    }
}