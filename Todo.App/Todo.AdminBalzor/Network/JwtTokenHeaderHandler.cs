using Blazored.LocalStorage;

namespace Todo.AdminBlazor.Network;

public class JwtTokenHeaderHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private IHttpContextAccessor _httpContextAccessor;

    public JwtTokenHeaderHandler(ILocalStorageService localStorage,IHttpContextAccessor httpContextAccessor)
    {
        _localStorage = localStorage;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!request.Headers.Contains("bearer"))
            {
                var savedToken =  _httpContextAccessor.HttpContext.Request.Cookies["access-token"];

                if (!string.IsNullOrWhiteSpace(savedToken))
                {
                    request.Headers.Add("authorization", "bearer" + savedToken);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception e)
        {
            
        }

        return default;
    }
}