using Todo.AdminBlazor.Helper;

namespace Todo.AdminBlazor.Network;

public class ClientSetter
{
    private CookieHelper _cookieHelper;
    private IHttpClientFactory _factory;
    public ClientSetter(CookieHelper cookieHelper,IHttpClientFactory factory)
    {
        _cookieHelper = cookieHelper;
        _factory = factory;
    }

    public HttpClient GetClient(string name)
    {
       var _client = _factory.HttpClientAsync(name);
        var token = _cookieHelper.GetCookie("access-token");
        if (!string.IsNullOrEmpty(token))
        {
            _client.DefaultRequestHeaders.Add("authorization", "Bearer  " + token);
        }

        return _client;
    }
}