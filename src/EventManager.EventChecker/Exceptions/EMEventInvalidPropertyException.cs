using System;

namespace EventManager.EventChecker.Exceptions
{
    public class EMEventInvalidPropertyException : System.Exception
    {
        public EMEventInvalidPropertyException(string message) : base(message)
        {

        }
        public EMEventInvalidPropertyException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
