using FMS.Framework.Core;
using FMS.Framework.Recurrence.Rules;

namespace FMS.Framework.Recurrence.Transforms
{
    public class BackupToWeekdayRule : ShiftWeekdayRule
    {
        public BackupToWeekdayRule(IRule innerRule, Weekday weekday, bool inclusive = true)
            : base(innerRule, weekday, inclusive)
        {
        }

        public override Date ResolveDate(Date referenceDate)
        {
            return RuleHelper.BackupToWeekday
            (
                InnerRule.ResolveDate(referenceDate),
                Weekday,
                Inclusive
            );
        }
    }
}
