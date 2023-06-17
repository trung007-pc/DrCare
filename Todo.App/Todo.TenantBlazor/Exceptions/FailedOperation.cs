using System;

namespace WebClient.Exceptions
{
    public class FailedOperation : Exception
    {
        public FailedOperation(string message) : base(message)
        {
            
        }
    }
}