using System;
using NUnit.Framework;
using FMS.Framework.Core;

namespace FMS.Framework.UnitTests.Core
{
    [TestFixture]
    public class UtcDateTimeTests
    {
        private const int Year = 2015;
        private const int Month = 2;
        private const int Day = 15;
        private const int Hour = 14;
        private const int Minute = 34;
        private const int Second = 57;
        private const int Millisecond = 999;

        [Test]
        public void UtcDateTime_Initialization()
        {
            var dateTime = new DateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
            
            var utcDateTime = new UtcDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
            var localTime = LocalDateTime.Now;

            Assert.AreEqual(dateTime.Ticks, utcDateTime.Ticks);
        }
    }
}
