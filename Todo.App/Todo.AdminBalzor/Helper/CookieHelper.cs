using Microsoft.JSInterop;

namespace Todo.AdminBlazor.Helper;

public class CookieHelper
{
    private HttpContext _context;
    private IJSRuntime _JSRuntime { get; set; }
    public CookieHelper(IHttpContextAccessor httpContextAccessor, IJSRuntime JSRuntime)
    {
        _context = httpContextAccessor.HttpContext;
        _JSRuntime = JSRuntime;
    }
    
    public string GetCookie(string name)
    {
        var value = _context.Request.Cookies[name];
        return value;
    }

    public async void SetCookie(string name,string value,int expiredDay)
    {
        await _JSRuntime.InvokeAsync<string>("blazorExtensions.WriteCookie", name, value, expiredDay);

    }

    public  void RemoveCookie(string name)
    {
        _context.Response.Cookies.Delete(name);
    }
}