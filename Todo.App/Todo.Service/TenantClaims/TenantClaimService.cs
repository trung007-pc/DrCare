using Todo.Contract.Claims;
using Todo.Contract.TenantClaims;
using Todo.Domain.TenantClaims;
using Todo.Localziration;
using Todo.MongoDb.Repositorys;

namespace Todo.Service.TenantClaims;

public class TenantClaimService : BaseService,ITenantClaimService
{
       private readonly UnitOfWork _unitOfWork;
    private  Localizer _localizer { get; set; }
    
    public TenantClaimService(UnitOfWork unitOfWork,Localizer localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }
    public async Task<List<TenantClaimDto>> GetListAsync()
    {
        var tenants = await _unitOfWork.TenantClaimRepository.ToListAsync();
       return  ObjectMapper.Map<List<TenantClaim>,List<TenantClaimDto>>(tenants);
    }

    public async Task UpdateClaims(List<ClaimDto> inputs)
    {
        var tenantClaimRepo = _unitOfWork.TenantClaimRepository;
       var oldClaims =  await tenantClaimRepo.ToListAsync();
       tenantClaimRepo.Entity.RemoveRange(oldClaims);
       return;
    }
}