using FMS.Framework.Interop;

namespace FMS.Framework.Core
{
    internal static class TziUtils
    {
        public static Tzi GetForcedStandardTimeRule(Tzi originalDstRule)
        {
            return new Tzi
            {
                Bias = originalDstRule.Bias,
                StandardBias = originalDstRule.Bias,
                DaylightBias = originalDstRule.DaylightBias
            };
        }

        public static bool IsCompletelyStandardTime(Tzi dstRule)
        {
            return dstRule.StandardDate.Month == 0 && dstRule.DaylightDate.Month == 0;
        }

        public static TimeDuration GetYearFinalUtcOffset(Tzi dstRule)
        {
            if (dstRule.DaylightDate.Month == 0)
            {
                return GetStandardTimeUtcOffset(dstRule);
            }

            if (dstRule.StandardDate.Month == 0)
            {
                return GetDaylightTimeUtcOffset(dstRule);
            }

            return dstRule.DaylightDate.Month < dstRule.StandardDate.Month
                ? GetStandardTimeUtcOffset(dstRule)
                : GetDaylightTimeUtcOffset(dstRule);
        }

        public static TimeDuration GetStandardTimeUtcOffset(Tzi dstRule)
        {
            return TimeDuration.FromMinutes(-(dstRule.Bias + dstRule.StandardBias));
        }

        public static TimeDuration GetDaylightTimeUtcOffset(Tzi dstRule)
        {
            return TimeDuration.FromMinutes(-(dstRule.Bias + dstRule.DaylightBias));
        }
    }
}
