using FMS.Framework.Core;
using FMS.Framework.Recurrence.Rules;

namespace FMS.Framework.Recurrence.Transforms
{
    public class AdvanceToWeekdayRule : ShiftWeekdayRule
    {
        public AdvanceToWeekdayRule(IRule innerRule, Weekday weekday, bool inclusive = true)
            : base(innerRule, weekday, inclusive)
        {
        }

        public override Date ResolveDate(Date referenceDate)
        {
            return RuleHelper.AdvanceToWeekday
            (
                InnerRule.ResolveDate(referenceDate),
                Weekday,
                Inclusive
            );
        }
    }
}
