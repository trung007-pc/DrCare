using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Todo.Contract.Claims;
using Todo.Contract.Users;
using Todo.Core.Consts.ErrorCodes;
using Todo.Core.DependencyRegistrationTypes;
using Todo.Core.Enums;
using Todo.Domain.Roles;
using Todo.Domain.UserRoles;
using Todo.Domain.Users;
using Todo.Localziration;
using Todo.MongoDb.Repositorys;
using Todo.Service.Exceptions;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Todo.Service.Users;

public class UserService : BaseService,IUserService,ITransientDependency
{
    private readonly UnitOfWork _unitOfWork;
    private IConfiguration _configuration;
    private  Localizer _localizer { get; set; }
    private UserManager<User> _userManager { get; set; }
    private RoleManager<Role> _rolerManager { get; set; }


    public UserService(UnitOfWork unitOfWork,Localizer localizer,UserManager<User> userManager,RoleManager<Role> rolerManager,IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
        _userManager = userManager;
        _configuration = configuration;
        _rolerManager = rolerManager;
    }
    
     public async Task<List<UserWithNavigationPropertiesDto>> GetListWithNavigationAsync()
        {
            var users = await _unitOfWork.UserRepository.GetListWithNavigationProperties();
            return ObjectMapper.Map<List<UserWithNavigationProperties>,List<UserWithNavigationPropertiesDto>>(users);
        }

     public Task<UserWithNavigationPropertiesDto> GetWithNavigationProperties(Guid id)
     {
         throw new NotImplementedException();
     }


     public async Task CreateWithNavigationPropertiesAsync(CreateUserDto input)
        {
            var dto = await CreateAsync(input);
            await UpdateRolesForUser(input.UserName, input.RoleIds);
          
        }


        public async Task UpdateWithNavigationPropertiesAsync(UpdateUserDto input, Guid id)
        {
            var item = await _userManager.FindByIdAsync(id.ToString());
            
            if (item == null)
            {
                throw new GlobalException(BaseErrorCode.NotFound, HttpStatusCode.BadRequest);
            }
            
            var user = ObjectMapper.Map(input, item);
            
            var result = await _userManager.UpdateAsync(user);
            
            if (!result.Succeeded)
            {
                
                throw new GlobalException(result.Errors?.FirstOrDefault().Code, HttpStatusCode.BadRequest);
            }
            
            
            if (input.IsSetPassword)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var reuslt = await _userManager.ResetPasswordAsync(user, token, input.Password);
                if (!reuslt.Succeeded)
                {
                    throw new GlobalException(reuslt.Errors?.FirstOrDefault().Code, HttpStatusCode.BadRequest);
                }
            }
            
            await UpdateRolesForUser(user.UserName, input.RoleIds);
          
        }




        public async Task DeleteWithNavigationPropertiesAsync(Guid id)
        {
            var item = await _userManager.FindByIdAsync(id.ToString());
            if (item == null) throw new GlobalException(BaseErrorCode.NotFound, HttpStatusCode.BadRequest);
            
            item.IsActive = false;
            var userResult = await _userManager.UpdateAsync(item);
            if (!userResult.Succeeded)
            {
                throw new GlobalException(userResult.Errors.FirstOrDefault().Code, HttpStatusCode.BadRequest);
            }
        }

        public async Task<List<UserDto>> GetListAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            
            return ObjectMapper.Map<List<User>, List<UserDto>>(users);
        }
        

        public async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            
            var user = ObjectMapper.Map<CreateUserDto, User>(input);
            
            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                throw new GlobalException(result.Errors?.FirstOrDefault().Code, HttpStatusCode.BadRequest);
            }

            return ObjectMapper.Map<User, UserDto>(user);
        }
        
        

        public async Task<UserDto> UpdateAsync(UpdateUserDto input, Guid id)
        {
            input.PhoneNumber = input.PhoneNumber.Trim();

            var item = await _userManager.FindByIdAsync(id.ToString());

            if (item == null)
            {
                throw new GlobalException(BaseErrorCode.NotFound, HttpStatusCode.BadRequest);
            }
            
            var user = ObjectMapper.Map(input, item);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new GlobalException(result.Errors?.FirstOrDefault().Code, HttpStatusCode.BadRequest);
            }

            return ObjectMapper.Map<User, UserDto>(user);
            ;
        }

        private (string Phone, string Code, string? Email) TrimText(string phone, string code, string email)
        {
            return (phone.Trim(), code.Trim(), email?.Trim());
        }



        public async Task DeleteAsync(Guid id)
        {
            var item = await _userManager.FindByIdAsync(id.ToString());
            if (item == null)
            {
                throw new GlobalException(BaseErrorCode.NotFound, HttpStatusCode.BadRequest);
            }
            item.IsDelete = true;
            _unitOfWork.UserRepository.Entity.Update(item);
            _unitOfWork.SaveChange();
        }

        public async Task<TokenDto> SignInAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            user.IsActive = true;
            if (user == null|| !user.IsActive|| user.IsDelete)
            {
                throw new GlobalException(BaseErrorCode.InvalidRequirement, HttpStatusCode.BadRequest);
            }


            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                throw new GlobalException(UserErrorCode.WrongPassword, HttpStatusCode.BadRequest);
            }

            string accessToken = await GenerateTokenByUser(user);

            return new TokenDto() {Token = accessToken};
        }

        public async Task<bool> ResetPasswordAsync(ResetRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new GlobalException(BaseErrorCode.NotFound, HttpStatusCode.BadRequest);
            }
            
            var token  = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result =   await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                throw new GlobalException(BaseErrorCode.TryMore, HttpStatusCode.BadRequest);
            }
            
            return true;
        }

        public async Task<List<ClaimDto>> GetClaims(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new GlobalException(BaseErrorCode.NotFound, HttpStatusCode.BadRequest);
            }

            var claims =(List<Claim>) await _userManager.GetClaimsAsync(user);
            
            return ObjectMapper.Map<List<Claim>, List<ClaimDto>>(claims);
        }

        public async Task<UserClaimsDto> GetClaimsAndRoleClaims(Guid id)
        {
           var userClaims  =  await GetClaims(id);
           var userRoleIds = await _unitOfWork.RoleRepository.GetRoleIds(id);
          var roleClaims = await  _unitOfWork.RoleClaimRepository.GetClaimsByRoleIds(userRoleIds);

          return new UserClaimsDto()
          {
              RoleClaims = ObjectMapper.Map<List<Claim>, List<ClaimDto>>(roleClaims),
              UserClaims = userClaims
          };
        }

        public async Task UpdateUserClaims(Guid id, List<CreateUpdateClaimDto> input)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new GlobalException(BaseErrorCode.NotFound, HttpStatusCode.BadRequest);
            }

            var oldUserClaims = _unitOfWork
                .UserClaimRepository.GetList(x => x.UserId == id)
                .Select(x => new Claim(x.ClaimType, x.ClaimValue));

           var newClaims = input.Select(x => new Claim(x.ClaimType,x.ClaimValue));
           await _userManager.RemoveClaimsAsync(user, oldUserClaims);
           await _userManager.AddClaimsAsync(user, newClaims);
        }


        public async Task UpdateRolesForUser(string userName, List<Guid> roleIds)
        {
           var roles = _unitOfWork.RoleRepository
                .Entity
                .Where(x => roleIds.Contains(x.Id))
                .AsNoTracking()
                .ToList();
           
            var user = await _userManager.FindByNameAsync(userName);
            var oldRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, oldRoles);
            await _userManager.AddToRolesAsync(user, roles.Select(x=>x.Name));
        }



    
        private async Task<string> GenerateTokenByUser(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>();
            var userRoles = await _unitOfWork
                .UserRepository.GetRoles(user.Id);
             foreach (var item in userRoles)
             {
                claims.Add(new Claim(ClaimTypes.Role, item.Name));
             }
            var roleClaims = await _unitOfWork
                .RoleClaimRepository.
                GetClaimsByRoleIds(userRoles.Select(x => x.Id).ToList());
            var userClaims =  _unitOfWork.UserClaimRepository
                .GetListAsync(x => x.UserId == user.Id)
                .Result
                .Select(x=>new Claim(x.ClaimType,x.ClaimValue));
            claims.AddRange(roleClaims);
            claims.AddRange(userClaims);
            claims.Add(new Claim(ClaimTypes.PrimarySid, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Surname,user.FirstName +" "+ user.LastName));
            
            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(168),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience =
                    false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    
}