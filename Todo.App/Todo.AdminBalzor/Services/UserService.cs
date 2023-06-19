using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Todo.AdminBlazor.Network;
using Todo.AdminBlazor.Security;
using Todo.Contract.Claims;
using Todo.Contract.Users;
using Todo.Core.DependencyRegistrationTypes;

namespace Todo.AdminBlazor.Services;

public class UserService : IUserService,ITransientDependency
{
    private readonly AuthenticationStateProvider  _authenticationStateProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private HttpClient _client;
    
    private IJSRuntime _JSRuntime { get; set; }

    

    public UserService(ILocalStorageService localStorage,IJSRuntime jsInterop,IHttpClientFactory factory
        ,AuthenticationStateProvider  authenticationStateProvider,IHttpContextAccessor httpContextAccessor) 
    {
        _authenticationStateProvider = authenticationStateProvider;
        _httpContextAccessor = httpContextAccessor;
        _JSRuntime = jsInterop;
        _client = factory.HttpClientAsync("DefaultClient");
    }


    public Task<UserDto> CreateAsync(CreateUserDto input)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateAsync(UpdateUserDto input, Guid id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteWithNavigationPropertiesAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserDto>> GetListAsync()
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TokenDto> SignInAsync(LoginRequestDto request)
    {
        throw new NotImplementedException();
    }

    Task<bool> IUserService.ResetPasswordAsync(ResetRequestDto request)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ClaimDto>> GetClaims(Guid id)
    {
        return await  _client.GetAPIAsync<List<ClaimDto>>("user/get-claims");
    }

    public async Task<UserClaimsDto> GetClaimsAndRoleClaims(Guid id)
    {
        return await  _client.GetAPIAsync<UserClaimsDto>($"user/get-claims-and-role-claims/{id}");
    }

    public async Task UpdateUserClaims(Guid id, List<CreateUpdateClaimDto> input)
    {
         await  _client.PutAPIAsync<Task>($"user/update-user-claims/{id}",input);
    }

    public async Task<List<UserWithNavigationPropertiesDto>> GetListWithNavigationAsync()
    {
       return await  _client.GetAPIAsync<List<UserWithNavigationPropertiesDto>>("user/get-list-with-nav-properties");
    }

    public Task<UserWithNavigationPropertiesDto> GetWithNavigationProperties(Guid id)
    {
        throw new NotImplementedException();
    }



    public async Task CreateWithNavigationPropertiesAsync(CreateUserDto input)
    {
         await  _client.PostAPIAsync<Task>("user/create-with-navigation-properties",input);
    }

    public async Task UpdateWithNavigationPropertiesAsync(UpdateUserDto input, Guid id)
    {
        await  _client.PutAPIAsync<Task>($"user/update-with-navigation-properties/{id}",input);
    }

    public async Task<TokenDto> LoginAsync(LoginRequestDto request)
    {
        var response = await _client.PostAPIAsync<TokenDto>("user/login", request);

        await _JSRuntime.InvokeAsync<string>("blazorExtensions.WriteCookie", "access-token", response.Token, 2);
        // await _localStorage.SetItemAsync("my-access-token", response.Token);
        ((ApiAuthenticationStateProvider) _authenticationStateProvider).MarkUserAsAuthenticated(request.UserName);
        return new TokenDto();
    }

    public Task ChangePasswordAsync(PasswordChangeRequestDto request)
    {
        throw new NotImplementedException();
    }

    public Task ResetPasswordAsync(ResetRequestDto request)
    {
        throw new NotImplementedException();
    }
}