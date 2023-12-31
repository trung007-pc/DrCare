﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Todo.BalzorServer.Security;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider 
{
        private readonly ILocalStorageService _localStorage;
        private readonly ISnackbar _snackbar;


    public ApiAuthenticationStateProvider(ILocalStorageService localStorage,
        ISnackbar snackbar
        )
    {
        _localStorage = localStorage;
        _snackbar = snackbar;
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = "";
        try
        {
            savedToken = await _localStorage.GetItemAsync<string>("my-access-token"); 
            
            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = ParseClaimsFromJwt1(savedToken);

            if (!CheckExpiredToken(claims))
            {
                await _localStorage.RemoveItemAsync("my-access-token");
                await _localStorage.RemoveItemAsync("my-refresh-token");


                NotificationMessage();
                
                Thread.Sleep(4000);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
          

            
            
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }
        catch (Exception e)
        {
        }
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    }

    public void NotificationMessage()
    {
        _snackbar.Add("Phiên làm việc của bạn đã hết.Hệ thống sẽ tự logout sau 4s nữa", Severity.Info, config =>
        {
            config.Icon = Icons.Custom.Brands.GitHub;
            config.IconColor = Color.Warning;
            config.IconSize = Size.Large;
        });
    }
    public bool CheckExpiredToken(IEnumerable<Claim> claims)
    {
        var expiredClaim =  claims.FirstOrDefault(x => x.Type == "exp");
        var epochTime = long.Parse(expiredClaim.Value);
        DateTime tokenTime = DateTime.UnixEpoch.AddSeconds(epochTime);
        if (tokenTime.ToLocalTime() < DateTime.Now)
        {
            return false;
        }

        return true;
    }
    

    public void MarkUserAsAuthenticated(string userName)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) }, "apiauth"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public async Task MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        await _localStorage.RemoveItemAsync("my-access-token");
        await _localStorage.RemoveItemAsync("my-refresh-token");
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        
        NotifyAuthenticationStateChanged(authState);
    }

    private IEnumerable<Claim> ParseClaimsFromJwt1(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();

        var decodedValue = handler.ReadJwtToken(jwt);
        return decodedValue.Claims;
    }


}