using NUnit.Framework;
using FMS.Framework.Core;

namespace FMS.Framework.UnitTests.Core
{
    [TestFixture]
    public class DateTimeValuesTests
    {
        private const int Year = 2015;
        private const int Month = 2;
        private const int Day = 15;
        private const int Hour = 14;
        private const int Minute = 34;
        private const int Second = 57;
        private const int Millisecond = 999;

        [Test]
        public void DateTimeValues_Initialization()
        {
            var dateTimeValues = new DateTimeValues(Year, Month, Day, Hour, Minute, Second, Millisecond);
            var timeDurationValues = new TimeDurationValues(245, 6, 23, 45, 999);
        }
    }
}
