using System;
using System.Collections.Generic;
using System.Text;

namespace FBM.Event.UniqueController.Exceptions
{
    public class FBMEventInvalidPropertyException : System.Exception
    {
        public FBMEventInvalidPropertyException(string message) : base(message)
        {

        }
        public FBMEventInvalidPropertyException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
