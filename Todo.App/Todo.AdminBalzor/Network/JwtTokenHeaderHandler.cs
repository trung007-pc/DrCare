using Blazored.LocalStorage;

namespace Todo.AdminBlazor.Network;

public class JwtTokenHeaderHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public JwtTokenHeaderHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!request.Headers.Contains("bearer"))
            {
                var savedToken = await _localStorage.GetItemAsync<string>("my-access-token");

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