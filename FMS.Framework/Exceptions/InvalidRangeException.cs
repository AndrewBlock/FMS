using System;

namespace FMS.Framework.Exceptions
{
    public class InvalidRangeException : FMSException
    {
        public InvalidRangeException()
        {
        }

        public InvalidRangeException(string message)
            : base(message)
        {
        }

        public InvalidRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
