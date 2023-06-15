using System.Linq.Dynamic.Core;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Todo.Contract.Tenants;
using Todo.Core.Consts.ErrorCodes;
using Todo.Core.DependencyRegistrationTypes;
using Todo.Domain.Tenants;
using Todo.Localziration;
using Todo.MongoDb.Repositorys;
using Todo.Service.Exceptions;

namespace Todo.Service.Tenants;

public class TenantService : BaseService,ITenantService,ITransientDependency
{
    private readonly UnitOfWork _unitOfWork;
    private  Localizer _localizer { get; set; }
    
    public TenantService(UnitOfWork unitOfWork,Localizer localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }
    public async Task<List<TenantDto>> GetListAsync()
    {
        var tenants = await _unitOfWork.TenantRepository.ToListAsync();
       return  ObjectMapper.Map<List<Tenant>,List<TenantDto>>(tenants);
    }

    public async Task<TenantDto> CreateAsync(CreateUpdateTenantDto input)
    {
        var tenantRepository = _unitOfWork.TenantRepository;
        HandleInput(input);
        
        if (tenantRepository.Entity.Any(x => x.Name == input.Name))
        {
            throw new GlobalException(_localizer[BaseErrorCode.ItemAlreadyExist], HttpStatusCode.BadRequest);
        }

        var tenant = ObjectMapper.Map<CreateUpdateTenantDto, Tenant>(input);

        await tenantRepository.Entity.AddAsync(tenant);
        _unitOfWork.SaveChange();
        
        return  ObjectMapper.Map<Tenant,TenantDto>(tenant);
    }

    public async Task<TenantDto> UpdateAsync(CreateUpdateTenantDto input, Guid id)
    {
        var tenantRepository = _unitOfWork.TenantRepository;
        HandleInput(input);

        var tenant = tenantRepository.Entity.FirstOrDefault(x => x.Id == id);
        if (tenant == null)
        {
            throw new GlobalException(_localizer[BaseErrorCode.NotFound], HttpStatusCode.BadRequest);
        }
        
        if (tenantRepository.Entity.Any(x => x.Name == input.Name))
        {
            throw new GlobalException(_localizer[BaseErrorCode.ItemAlreadyExist], HttpStatusCode.BadRequest);
        }

        tenant = ObjectMapper.Map(input,tenant);

        await tenantRepository.Entity.AddAsync(tenant);
        _unitOfWork.SaveChange();
        
        return  ObjectMapper.Map<Tenant,TenantDto>(tenant);
    }

    public async Task DeleteAsync(Guid id)
    {
        var tenantRepository = _unitOfWork.TenantRepository;

        var tenant = await tenantRepository.Entity.FirstOrDefaultAsync(x => x.Id == id);
        if (tenant == null)
        {
            throw new GlobalException(_localizer[BaseErrorCode.NotFound], HttpStatusCode.BadRequest);
        } 
        tenantRepository.Entity.Remove(tenant);
        _unitOfWork.SaveChange();
    }

    public void HandleInput(CreateUpdateTenantDto input)
    {
        input.Name = input.Name.Trim();
    }
}