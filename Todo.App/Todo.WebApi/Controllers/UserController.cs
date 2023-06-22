using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Contract.Claims;
using Todo.Contract.Users;
using Todo.Service.Users;

namespace Todo.App.Controllers;

[ApiController]
[Route("api/user/")]
[Authorize]
public class UserController : IUserService
{
    private UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }


    [HttpPost]
    public async Task<UserDto> CreateAsync(CreateUserDto input)
    {
       return  await _userService.CreateAsync(input);
    }
    
    [HttpPut]
    public async Task<UserDto> UpdateAsync(UpdateUserDto input, Guid id)
    {
        return  await _userService.UpdateAsync(input,id);
    }

    [HttpDelete]
    [Route("delete-with-navigation-properties/{id}")]
    public Task DeleteWithNavigationPropertiesAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public  async Task<List<UserDto>> GetListAsync()
    {
        return await _userService.GetListAsync();
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _userService.DeleteAsync(id);
    }

    [HttpPost]
    [Route("sign-in")]
    public Task<TokenDto> SignInAsync(LoginRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("reset-password")]
    Task<bool> IUserService.ResetPasswordAsync(ResetRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("get-claims/{id}")]
    public async Task<List<ClaimDto>> GetClaims(Guid id)
    {
        return await _userService.GetClaims(id);
    }

    [HttpGet]
    [Route("get-claims-and-role-claims/{id}")]
    public async Task<UserClaimsDto> GetClaimsAndRoleClaims(Guid id)
    {
        return await _userService.GetClaimsAndRoleClaims(id);
    }

    [HttpPut]
    [Route("update-user-claims/{id}")]
    public async Task UpdateUserClaims(Guid id, List<CreateUpdateClaimDto> input)
    {
        await _userService.UpdateUserClaims(id, input);
    }

    [HttpGet]
    [Route("get-list-with-nav-properties")]
    public async Task<List<UserWithNavigationPropertiesDto>> GetListWithNavigationAsync()
    {
       return  await _userService.GetListWithNavigationAsync();
    }

    
    [HttpGet]
    [Route("get-with-nav-properties")]
    public Task<UserWithNavigationPropertiesDto> GetWithNavigationProperties(Guid id)
    {
        throw new NotImplementedException();
    }


    [HttpPost]
    [Route("create-with-navigation-properties")]
    public async Task CreateWithNavigationPropertiesAsync(CreateUserDto input)
    {
        await _userService.CreateWithNavigationPropertiesAsync(input);
    }
    
    [HttpPut]
    [Route("update-with-navigation-properties/{id}")]
    public async Task UpdateWithNavigationPropertiesAsync(UpdateUserDto input, Guid id)
    {
        await _userService.UpdateWithNavigationPropertiesAsync(input, id);
    }
    
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<TokenDto> LoginAsync(LoginRequestDto request)
    {
        return await _userService.SignInAsync(request);
    }
    
    
    [HttpPost]
    [Route("reset-password")]
    public async Task ResetPasswordAsync(ResetRequestDto request)
    {
        await _userService.ResetPasswordAsync(request);
    }
}