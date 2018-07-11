using FMS.Framework.Core;
using FMS.Framework.Recurrence.Iterators;
using FMS.Framework.Recurrence.Rules;

namespace FMS.Framework.Recurrence.Transforms
{
    public abstract class TransformRule : IRule
    {
        protected TransformRule(IRule innerRule)
        {
            if (innerRule == null)
                throw RuleHelper.CreateInvalidRuleException(nameof(innerRule));

            InnerRule = innerRule;
        }

        public IRule InnerRule { get; }

        public RecurrenceType RecurrenceType => InnerRule.RecurrenceType;
        public IRecurrenceIterator GetIterator(Date dateStart) => InnerRule.GetIterator(dateStart);

        public abstract Date ResolveDate(Date referenceDate);
    }
}
