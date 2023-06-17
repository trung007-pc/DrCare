using System.IdentityModel.Tokens.Jwt;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Todo.AdminBlazor.Network;
using Todo.Core.Consts.Permissions;

namespace Todo.AdminBlazor.Services;

public class BaseService : ComponentBase
{
    protected HttpClient _client;
    protected readonly ILocalStorageService _localStorage;


    [CascadingParameter]
    public Task<AuthenticationState> AuthState { get; set; }
    public  BaseService(IHttpClientFactory factory,ILocalStorageService localStorage , IHttpContextAccessor httpContextAccessor)
    {
        try
        {
           var c =   localStorage.GetItemAsync<string>("my-access-token").Result;
            _client = factory.HttpClientAsync("defaultClient");
           

            var token = "";
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(token);

                var tenant = decodedValue.Claims.FirstOrDefault(x => x.Type == ExtendClaimTypes.Tenant);

                _client.DefaultRequestHeaders.Add("authorization", "bearer " + token);
            }
           

        }
        catch (Exception e)
        {
            
        }

    }

    
    
    
}