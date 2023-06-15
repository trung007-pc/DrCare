using System.Net;
using System.Text.Json;
using Todo.Service.Exceptions;

namespace Todo.App;

public class GlobalErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public static IConfiguration _configuration;

    public GlobalErrorHandlingMiddleware(RequestDelegate next,IConfiguration config)
    {
        _next = next;
        _configuration = config;

    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status = HttpStatusCode.InternalServerError;
        var stackTrace = string.Empty;
        string message = "";

            
        var exceptionType = exception.GetType();

        if (exceptionType == typeof(GlobalException))
        {
            var globalException = (GlobalException) exception;
            message = globalException.Message;
            status = globalException.Status;
            stackTrace = globalException.StackTrace;
        }
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) status;
        var exceptionResult = JsonSerializer.Serialize(new {message = message, status = status});
        return context.Response.WriteAsync(exceptionResult);
    }
}