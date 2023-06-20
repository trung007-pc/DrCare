using System.Linq.Dynamic.Core;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Todo.Contract.Claims;
using Todo.Contract.Tenants;
using Todo.Core.Consts.ErrorCodes;
using Todo.Core.Consts.Permissions;
using Todo.Core.DependencyRegistrationTypes;
using Todo.Domain.RoleClaims;
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
    private UserManager<User> _userManager { get; set; }
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
        
        if (tenantRepository.Entity.Any(x => x.Name == input.Name))
        {
            throw new GlobalException(_localizer[BaseErrorCode.ItemAlreadyExist], HttpStatusCode.BadRequest);
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
        
        if (tenantRepository.Entity.Any(x => x.Name == input.Name  && x.Id!= id))
        {
            throw new GlobalException(_localizer[BaseErrorCode.ItemAlreadyExist], HttpStatusCode.BadRequest);
        }

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
        var role = await  _unitOfWork.RoleRepository.Entity.FirstOrDefaultAsync(x=>x.Id == id);
        if (role == null)
        {
            throw new GlobalException(_localizer[BaseErrorCode.NotFound], HttpStatusCode.BadRequest);
        }
            
        if (CheckAllowedClaimType(claims))
        {
            throw new GlobalException(_localizer[BaseErrorCode.InvalidRequirement], HttpStatusCode.BadRequest);
        }
            
        var oldClaims = _unitOfWork.RoleClaimRepository
            .Entity.Where(x => x.RoleId == id);
        _unitOfWork.RoleClaimRepository.Entity.RemoveRange(oldClaims);
            
        var newRoleClaims = new List<RoleClaim>();
        foreach (var item in claims)
        {
            newRoleClaims.Add(new RoleClaim()
            {
                RoleId = id,
                ClaimType = item.ClaimType,
                ClaimValue = item.ClaimValue
            });
        }
        _unitOfWork.RoleClaimRepository.Entity.AddRange(newRoleClaims);
        _unitOfWork.SaveChange();
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
    }
}