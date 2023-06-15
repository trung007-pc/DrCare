using System;

namespace WebClient.Exceptions
{
    public class TooManyRequests : Exception
    {
        public TooManyRequests(string message) : base(message)
        {
            
        }
    }
}