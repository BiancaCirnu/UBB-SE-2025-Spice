using System;

namespace SteamProfile.Data.Exceptions
{
    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException(string message) : base(message) { }
        public DatabaseOperationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
} 