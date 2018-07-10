using NUnit.Framework;
using FMS.Framework.Core;

namespace FMS.Framework.UnitTests.Core
{
    [TestFixture]
    public class TimeZoneTests
    {
        [Test]
        public void TimeZone_Initialization()
        {
            var timeZones = TimeZone.TimeZones;
        }
    }
}
