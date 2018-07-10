using System;

namespace FMS.Framework.Exceptions
{
    public class InternalOperationException : FMSException
    {
        public InternalOperationException()
        {
        }

        public InternalOperationException(string message)
            : base(message)
        {
        }

        public InternalOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
