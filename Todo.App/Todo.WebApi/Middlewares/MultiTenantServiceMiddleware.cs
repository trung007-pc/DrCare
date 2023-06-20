using Todo.MongoDb.PostgreSQL;

namespace Todo.App.Middlewares;

public class MultiTenantServiceMiddleware : IMiddleware
{
    private readonly TenantContext _tenantContext;
    private readonly ILogger<MultiTenantServiceMiddleware> logger;
    
    public MultiTenantServiceMiddleware(
        TenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }
    
    public  async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var headerValue = context.Request.Headers["Tenant-Id"].ToString();
        if (Guid.TryParse(headerValue, out Guid tenantId))
        {
            _tenantContext.TenantId = tenantId;
        }
        await next(context);
       
      
    }
}