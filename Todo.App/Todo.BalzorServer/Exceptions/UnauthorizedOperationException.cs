using System;

namespace WebClient.Exceptions
{
    public class UnauthorizedOperationException : Exception
    {
        public UnauthorizedOperationException(string message) : base(message)
        {
            
        }
    }
}