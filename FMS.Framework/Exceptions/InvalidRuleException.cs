using System;

namespace FMS.Framework.Exceptions
{
    public class InvalidRuleException : FMSException
    {
        public InvalidRuleException()
        {
        }

        public InvalidRuleException(string message)
            : base(message)
        {
        }

        public InvalidRuleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
