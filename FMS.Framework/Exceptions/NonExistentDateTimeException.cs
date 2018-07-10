using System;

namespace FMS.Framework.Exceptions
{
    public class NonExistentDateTimeException : FMSException
    {
        public NonExistentDateTimeException()
        {
        }

        public NonExistentDateTimeException(string message)
            : base(message)
        {
        }

        public NonExistentDateTimeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
