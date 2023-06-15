using System.Net;

namespace Todo.Service.Exceptions;

public class GlobalException : Exception
{
    public HttpStatusCode Status= HttpStatusCode.BadRequest;
        
    public GlobalException(string message,HttpStatusCode status) : base(message)
    {
        Status = status;
    }
}