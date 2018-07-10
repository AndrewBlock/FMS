using System;

namespace FMS.Framework.Exceptions
{
    public abstract class FMSException : Exception
    {
        public FMSException()
        {
        }

        public FMSException(string message)
            : base(message)
        {
        }

        public FMSException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
