using Todo.MongoDb.PostgreSQL;

namespace Todo.App.Middlewares;

public class MultiTenantServiceMiddleware : IMiddleware
{
    private readonly TenantContext _tenantContext;
    private readonly ILogger<MultiTenantServiceMiddleware> logger;
    private readonly TodoContext _db;
    
    public MultiTenantServiceMiddleware(
        TenantContext tenantContext,TodoContext db)
    {
        _tenantContext = tenantContext;
        _db = db;
    }
    
    public  async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var headerValue = context.Request.Headers["Tenant-Id"].ToString();
        if (Guid.TryParse(headerValue, out Guid tenantId))
        {
            if (!_db.Tenants.Any(x => x.Id == tenantId))
            {
                throw new Exception();
            }

            _tenantContext.TenantId = tenantId;
             
           
        }
        await next(context);
       
      
    }
}