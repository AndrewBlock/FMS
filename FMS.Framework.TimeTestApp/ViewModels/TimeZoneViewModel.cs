using System.Collections.Generic;
using FMS.Framework.Core;

namespace FMS.Framework.TimeTestApp.ViewModels
{
    public class TimeZoneViewModel : ViewModelBase
    {
        private UtcDateTime _utcDateTime;
        private TimeZone _timeZone;
        private LocalDateTime _localDateTime;

        public TimeZoneViewModel(string timeZoneIdentifier)
        {
            _utcDateTime = UtcDateTime.Now;
            _timeZone = TimeZone.LookupTimeZoneByIdentifier(timeZoneIdentifier);
            UpdateLocalDateTime();
        }

        public IEnumerable<TimeZone> TimeZones => TimeZone.TimeZones;

        public TimeZone TimeZone
        {
            get
            {
                return _timeZone;
            }
            set
            {
                if (value == _timeZone)
                    return;

                _timeZone = value;
                NotifyPropertyChanged("TimeZone");

                UpdateLocalDateTime();
            }
        }

        public LocalDateTime LocalDateTime
        {
            get
            {
                return _localDateTime;
            }
            set
            {
                _localDateTime = value;
                NotifyPropertyChanged("LocalDateTime");
            }
        }

        public UtcDateTime UtcDateTime
        {
            get
            {
                return _utcDateTime;
            }
            set
            {
                if (Equals(value, _utcDateTime))
                    return;

                _utcDateTime = value;
                NotifyPropertyChanged("UtcDateTime");

                UpdateLocalDateTime();
            }
        }

        private void UpdateLocalDateTime()
        {
            LocalDateTime = new LocalDateTime(UtcDateTime, TimeZone);
        }
    }
}
