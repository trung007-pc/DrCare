using System;

namespace WebClient.Exceptions
{
    public class DbConnectionException : Exception
    {
        public DbConnectionException(string message) : base(message)
        {
            
        }
    }
}