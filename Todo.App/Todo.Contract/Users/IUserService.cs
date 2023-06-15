using Todo.Contract.Claims;

namespace Todo.Contract.Users;

public interface IUserService
{
    public Task<List<UserWithNavigationPropertiesDto>> GetListWithNavigationAsync();


    public Task<UserWithNavigationPropertiesDto> GetWithNavigationProperties(Guid id);
        
    public Task CreateWithNavigationPropertiesAsync(CreateUserDto input);
    public Task UpdateWithNavigationPropertiesAsync(UpdateUserDto input,Guid id);

        
    public Task DeleteWithNavigationPropertiesAsync(Guid id);
    public Task<List<UserDto>> GetListAsync();

    public Task<UserDto> CreateAsync(CreateUserDto input);
    public Task<UserDto> UpdateAsync(UpdateUserDto input,Guid id);
    public Task DeleteAsync(Guid id);
    
    public Task<TokenDto> SignInAsync(LoginRequestDto request);
    public Task<bool> ResetPasswordAsync(ResetRequestDto request);
    public Task<List<ClaimDto>> GetClaims(Guid id);
    public Task<UserClaimsDto> GetClaimsAndRoleClaims(Guid id);

    public Task UpdateUserClaims(Guid id, List<CreateUpdateClaimDto> input);
}