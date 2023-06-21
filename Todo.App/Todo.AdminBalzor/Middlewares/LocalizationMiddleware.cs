using System.Globalization;
using Todo.AdminBlazor.Helper;

namespace Todo.AdminBlazor.Middlewares;

public class LocalizationMiddleware : IMiddleware
{
    private HttpContext _context;
    public  LocalizationMiddleware(IHttpContextAccessor httpContextAccessor)
    {
        _context = httpContextAccessor.HttpContext;
    }
    
  
    private static bool DoesCultureExist(string cultureName)
    {
        return GlobalSetting.GetCultureCodes().Any(x => x.ToLower() == cultureName.ToLower());
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
       
            var cultureKey = _context.Request.Cookies["lang"];

            if (!string.IsNullOrEmpty(cultureKey))
            {
                if (DoesCultureExist(cultureKey))
                {
                    var culture = new CultureInfo(cultureKey);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
            }
            else
            {
                context.Response.Cookies.Append("lang", "en-Us");
            };
            
            await next(context);
    }
}