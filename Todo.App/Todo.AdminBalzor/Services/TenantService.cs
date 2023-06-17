using Blazored.LocalStorage;
using Todo.AdminBlazor.Network;
using Todo.Contract.Tenants;
using Todo.Contract.Tenants;
using Todo.Core.DependencyRegistrationTypes;

namespace Todo.AdminBlazor.Services;

public class TenantService : BaseService,ITenantService,ITransientDependency
{
    
    public TenantService(ILocalStorageService localStorage,IHttpClientFactory factory,IHttpContextAccessor httpContextAccessor) : base(factory,localStorage,httpContextAccessor)
    {
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
}