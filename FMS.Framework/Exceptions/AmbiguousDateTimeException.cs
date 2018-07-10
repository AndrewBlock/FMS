using System;

namespace FMS.Framework.Exceptions
{
    public class AmbiguousDateTimeException : FMSException
    {
        public AmbiguousDateTimeException()
        {
        }

        public AmbiguousDateTimeException(string message)
            : base(message)
        {
        }

        public AmbiguousDateTimeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
