using System.IdentityModel.Tokens.Jwt;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Todo.AdminBlazor.Helper;
using Todo.AdminBlazor.Network;
using Todo.Core.Consts.Permissions;

namespace Todo.AdminBlazor.Services;

public class BaseService 
{
    protected HttpClient _client;

    [Inject]
    private CookieHelper _helper { get; set; }

    public  BaseService(IHttpClientFactory factory )
    {
        if (_client is  null)
        {
            _client = factory.HttpClientAsync("DefaultClient");

            var token = _helper.GetCookie("access-token");
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Add("authorization", "Bearer  " + token);
            }
        }
     

    }

    
    
    
}