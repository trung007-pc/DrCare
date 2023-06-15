namespace Todo.Contract.Tenants;

public interface ITenantService
{
    Task<List<TenantDto>> GetListAsync();
    Task<TenantDto> CreateAsync(CreateUpdateTenantDto input);
    Task<TenantDto> UpdateAsync(CreateUpdateTenantDto input,Guid id);
    Task DeleteAsync(Guid id);
}