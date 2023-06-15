using Microsoft.AspNetCore.Mvc;
using Todo.Contract.Tenants;
using Todo.Service.Tenants;

namespace Todo.App.Controllers;


[ApiController]
[Route("api/tenant/")]
public class TenantController : ITenantService
{
    private TenantService _tenantService;
    public TenantController(TenantService tenantService)
    {
        _tenantService = tenantService;
    }
    
    [HttpGet]
    [Route("get-list")]
    public async Task<List<TenantDto>> GetListAsync()
    {
        return await _tenantService.GetListAsync();
    }

    [HttpPost]
    public async Task<TenantDto> CreateAsync(CreateUpdateTenantDto input)
    {
        return await _tenantService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<TenantDto> UpdateAsync(CreateUpdateTenantDto input, Guid id)
    {
        return await _tenantService.UpdateAsync(input,id);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task DeleteAsync(Guid id)
    {
         await _tenantService.DeleteAsync(id);
    }
}