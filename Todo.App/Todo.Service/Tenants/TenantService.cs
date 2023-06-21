using System.Linq.Dynamic.Core;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Todo.Contract.Claims;
using Todo.Contract.Tenants;
using Todo.Core.Consts.ErrorCodes;
using Todo.Core.Consts.Permissions;
using Todo.Core.DependencyRegistrationTypes;
using Todo.Domain.RoleClaims;
using Todo.Domain.TenantClaims;
using Todo.Domain.Tenants;
using Todo.Domain.Users;
using Todo.Localziration;
using Todo.MongoDb.Repositorys;
using Todo.Service.Exceptions;
using Todo.Service.Users;

namespace Todo.Service.Tenants;

public class TenantService : BaseService,ITenantService,ITransientDependency
{
    private readonly UnitOfWork _unitOfWork;
    private  Localizer _localizer { get; set; }
    private SharedUserService _sharedUserService { get; set; }

    
    public TenantService(UnitOfWork unitOfWork,Localizer localizer,SharedUserService sharedUserService)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
        
        _sharedUserService = sharedUserService;
        _sharedUserService.InjectUnitOfWork(_unitOfWork);
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
        
        if (tenantRepository.Entity.Any(x => x.PhoneNumber == input.PhoneNumber))
        {
            throw new GlobalException(_localizer[TenantErrorCode.PhoneNumberAlreadyExist], HttpStatusCode.BadRequest);
        }
       

        var tenant = ObjectMapper.Map<CreateUpdateTenantDto, Tenant>(input);

        await tenantRepository.Entity.AddAsync(tenant);
        await _sharedUserService.CreateUserAndRoleDefaultToTenant(tenant);
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
        
        input.PhoneNumber = tenant.PhoneNumber;
        tenant = ObjectMapper.Map(input,tenant);

        tenantRepository.Entity.Update(tenant);
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

        tenant.IsDeleted = true;
        tenantRepository.Entity.Update(tenant);
        _unitOfWork.SaveChange();
    }
    
    public async Task UpdateClaims(Guid id, List<CreateUpdateClaimDto> claims)
    {
        var tenant = await  _unitOfWork.TenantRepository.Entity.FirstOrDefaultAsync(x=>x.Id == id);
        if (tenant == null)
        {
            throw new GlobalException(_localizer[BaseErrorCode.NotFound], HttpStatusCode.BadRequest);
        }
            
        if (CheckAllowedClaimType(claims))
        {
            throw new GlobalException(_localizer[BaseErrorCode.InvalidRequirement], HttpStatusCode.BadRequest);
        }
            
        var oldClaims = _unitOfWork.TenantClaimRepository
            .Entity.Where(x => x.TenantId == id);
        _unitOfWork.TenantClaimRepository.Entity.RemoveRange(oldClaims);
            
        var newTenantClaims = new List<TenantClaim>();
        foreach (var item in claims)
        {
            newTenantClaims.Add(new TenantClaim()
            {
                TenantId = id,
                ClaimType = item.ClaimType,
                ClaimValue = item.ClaimValue
            });
        }
        _unitOfWork.TenantClaimRepository.Entity.AddRange(newTenantClaims);
        _unitOfWork.SaveChange();
    }

    public async Task<List<ClaimDto>> GetClaims(Guid tenantId)
    {
        var tenant = await  _unitOfWork.TenantRepository
            .Entity.FirstOrDefaultAsync(x => x.Id == tenantId);
        if (tenant == null)
        {
            throw new GlobalException(_localizer[BaseErrorCode.NotFound], HttpStatusCode.BadRequest);
        }

       var claims = await _unitOfWork.TenantClaimRepository
            .Entity
            .Where(x => x.TenantId == tenantId)
            .Select(x=>new ClaimDto()
            {
                Type = x.ClaimType,
                Value = x.ClaimValue
            }).ToListAsync();
       
        return  claims;
    }

    private bool CheckAllowedClaimType(List<CreateUpdateClaimDto> Claims)
    {
        var types = new List<string>()
        {
            ExtendClaimTypes.Permission
        };
        
        var invalidTypes = Claims.Select(x=>x.ClaimType)
            .Distinct()
            .Except(types);
        if (invalidTypes.Any())
        {
            return true;
        }
        
        return false;
    }

    public void HandleInput(CreateUpdateTenantDto input)
    {
        input.Name = input.Name.Trim();
        if (input.StartDate >= input.EndDate || input.EndDate <= DateTime.Now && input.IsActive)
        {
            throw new GlobalException(_localizer[BaseErrorCode.InvalidTimeRange], HttpStatusCode.BadRequest);
        }
        
    }
}