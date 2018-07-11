using FMS.Framework.Core;
using FMS.Framework.Recurrence.Rules;
using FMS.Framework.Utils;

namespace FMS.Framework.Recurrence.Transforms
{
    public abstract class ShiftWeekdayRule : TransformRule
    {
        protected ShiftWeekdayRule(IRule innerRule, Weekday weekday, bool inclusive)
            : base(innerRule)
        {
            if (!EnumUtils.IsValidEnumValue(weekday))
                throw RuleHelper.CreateInvalidRuleException(nameof(weekday));

            Weekday = weekday;
            Inclusive = inclusive;
        }
        
        public Weekday Weekday { get; }
        public bool Inclusive { get; }
    }
}
