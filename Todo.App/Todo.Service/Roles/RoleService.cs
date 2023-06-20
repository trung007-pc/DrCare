using System.Linq.Dynamic.Core;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todo.Contract.Claims;
using Todo.Contract.RoleClaims;
using Todo.Contract.Roles;
using Todo.Core.Consts.ErrorCodes;
using Todo.Core.Consts.Permissions;
using Todo.Core.DependencyRegistrationTypes;
using Todo.Domain.RoleClaims;
using Todo.Domain.Roles;
using Todo.Localziration;
using Todo.MongoDb.Repositorys;
using Todo.Service.Exceptions;

namespace Todo.Service.Roles;

public class RoleService :BaseService,IRoleService,ITransientDependency
{
       private readonly UnitOfWork _unitOfWork;
       private RoleManager<Role> _roleManager;

       private  Localizer L { get; set; }


        public RoleService(UnitOfWork unitOfWork,RoleManager<Role> roleManager,Localizer localizer) 
        {
            _unitOfWork = unitOfWork;
            L = localizer;
            _roleManager = roleManager;
        }

        public async Task<List<RoleDto>> GetListAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesDto = ObjectMapper.Map<List<Role>, List<RoleDto>>(roles);
            return rolesDto;
        }

        public async Task<RoleDto> CreateAsync(CreateUpdateRoleDto input)
        {
            HandleInput(input);
            var role = ObjectMapper.Map<CreateUpdateRoleDto, Role>(input);
         
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new GlobalException(result.Errors.FirstOrDefault().Code, HttpStatusCode.BadRequest);
            }

            return ObjectMapper.Map<Role,RoleDto>(role);
        }

        public async Task<RoleDto> UpdateAsync(CreateUpdateRoleDto input, Guid id)
        {
            input.Name = input.Name.Trim().ToUpper();
            input.Code = input.Code?.Trim().ToUpper();
            var item = await _roleManager.FindByIdAsync(id.ToString());

            if (item == null)
            {
                throw new GlobalException(BaseErrorCode.NotFound, HttpStatusCode.BadRequest);
            }
            
            var role = ObjectMapper.Map(input, item);
            var result = await _roleManager.UpdateAsync(role);
            
            if (!result.Succeeded)
            {
                throw new GlobalException(result.Errors?.FirstOrDefault().Code, HttpStatusCode.BadRequest);

            }
            
            return  ObjectMapper.Map<Role,RoleDto>(role);
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new GlobalException(BaseErrorCode.NotFound, HttpStatusCode.BadRequest);
            }

            var result = await _roleManager.DeleteAsync(role);
            
            if (!result.Succeeded)
            {
                throw new GlobalException(result.Errors?.FirstOrDefault().Code, HttpStatusCode.BadRequest);

            }
        }



        public async Task UpdateClaimsToRole(Guid id, List<CreateUpdateClaimDto> claims)
        {
            var role = await  _unitOfWork.RoleRepository.Entity.FirstOrDefaultAsync(x=>x.Id == id);
            if (role == null)
            {
                throw new GlobalException(L[BaseErrorCode.NotFound], HttpStatusCode.BadRequest);
            }
            
            if (CheckAllowedClaimType(claims))
            {
                throw new GlobalException(L[BaseErrorCode.InvalidRequirement], HttpStatusCode.BadRequest);
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

        public async Task<List<ClaimDto>> GetClaims(Guid roleId)
        {
            var role = await _unitOfWork.RoleRepository
                .Entity.FirstOrDefaultAsync(x => x.Id == roleId);
            if (role == null)
            {
                throw new GlobalException(L[BaseErrorCode.NotFound], HttpStatusCode.BadRequest);
            }
            var result = (List<Claim>)  await _roleManager.GetClaimsAsync(role);

            

            return  ObjectMapper.Map<List<Claim>,List<ClaimDto>>(result);
        }

        public async Task CreateWithClaimsAsync(CreateUpdateRoleDto input)
        {
            var role = await CreateAsync(input);
            
            
            if (CheckAllowedClaimType(input.Claims))
            {
                throw new GlobalException(L[BaseErrorCode.InvalidRequirement], HttpStatusCode.BadRequest);
            }
            
            var roleClaims = new List<RoleClaim>();
            foreach (var item in input.Claims)
            {
                roleClaims.Add(new RoleClaim()
                {
                    RoleId = role.Id,
                    ClaimType = item.ClaimType,
                    ClaimValue = item.ClaimValue
                });
            }
            
            _unitOfWork.RoleClaimRepository.Entity.AddRange(roleClaims);
            _unitOfWork.SaveChange();
           
        }

        public async Task UpdateWithClaimsAsync(CreateUpdateRoleDto input, Guid id)
        {
           await  UpdateAsync(input, id);
           var oldClaims = _unitOfWork.RoleClaimRepository
               .Entity.Where(x => x.RoleId == id);
           _unitOfWork.RoleClaimRepository.Entity.RemoveRange(oldClaims);
            
           var newRoleClaims = new List<RoleClaim>();
           foreach (var item in input.Claims)
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

        
        private void HandleInput(CreateUpdateRoleDto input)
        {
            input.Name = input.Name.Trim();
            input.Code = input.Code?.Trim();
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
}