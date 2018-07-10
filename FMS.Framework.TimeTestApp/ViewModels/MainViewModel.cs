using System;
using FMS.Framework.Core;

namespace FMS.Framework.TimeTestApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private UtcDateTime _utcDateTime = UtcDateTime.Now;

        private double _hourScrollValue = 0;
        private double _minuteScrollValue = 0;
        private double _secondScrollValue = 0;

        private static readonly TimeDuration HourIncrement = new TimeDuration(0, 1, 0, 0, 0);
        private static readonly TimeDuration MinuteIncrement = new TimeDuration(0, 0, 1, 0, 0);
        private static readonly TimeDuration SecondIncrement = new TimeDuration(0, 0, 0, 1, 0);

        public MainViewModel()
        {
            TimeZone1 = new TimeZoneViewModel("Pacific Standard Time");
            TimeZone2 = new TimeZoneViewModel("Mountain Standard Time");
            TimeZone3 = new TimeZoneViewModel("Canada Central Standard Time");
            TimeZone4 = new TimeZoneViewModel("Central Standard Time");
            TimeZone5 = new TimeZoneViewModel("Eastern Standard Time");
            TimeZone6 = new TimeZoneViewModel("Atlantic Standard Time");
            TimeZone7 = new TimeZoneViewModel("Newfoundland Standard Time");
            TimeZone8 = new TimeZoneViewModel("W. Europe Standard Time");
            TimeZone9 = new TimeZoneViewModel("Tasmania Standard Time");

            UpdateTimeZoneViewModels();
        }

        public UtcDateTime UtcDateTime
        {
            get => _utcDateTime;
            set
            {
                if (Equals(value, _utcDateTime))
                    return;

                _utcDateTime = value;

                NotifyPropertyChanged("UtcDateTime");
                NotifyPropertyChanged("UtcDate");
                NotifyPropertyChanged("UtcHour");
                NotifyPropertyChanged("UtcMinute");
                NotifyPropertyChanged("UtcSecond");

                UpdateTimeZoneViewModels();
            }
        }

        public UtcDateTime UtcDate
        {
            get => UtcDateTime;
            set
            {
                var updatedDateTime = new UtcDateTime
                (
                    value.Year,
                    value.Month,
                    value.Day,
                    UtcDateTime.Hour,
                    UtcDateTime.Minute,
                    UtcDateTime.Second,
                    UtcDateTime.Millisecond
                );

                if (!Equals(updatedDateTime, UtcDateTime))
                    UtcDateTime = updatedDateTime;
            }
        }

        public int UtcHour
        {
            get => UtcDateTime.Hour;
            set
            {
                try
                {
                    var updatedDateTime = new UtcDateTime
                    (
                        UtcDateTime.Year,
                        UtcDateTime.Month,
                        UtcDateTime.Day,
                        value,
                        UtcDateTime.Minute,
                        UtcDateTime.Second,
                        UtcDateTime.Millisecond
                    );

                    if (!Equals(updatedDateTime, UtcDateTime))
                        UtcDateTime = updatedDateTime;
                }
                catch (ArgumentException)
                {}
            }
        }

        public int UtcMinute
        {
            get => UtcDateTime.Minute;
            set
            {
                var updatedDateTime = new UtcDateTime
                (
                    UtcDateTime.Year,
                    UtcDateTime.Month,
                    UtcDateTime.Day,
                    UtcDateTime.Hour,
                    value,
                    UtcDateTime.Second,
                    UtcDateTime.Millisecond
                );

                if (!Equals(updatedDateTime, UtcDateTime))
                    UtcDateTime = updatedDateTime;
            }
        }

        public int UtcSecond
        {
            get => UtcDateTime.Second;
            set
            {
                var updatedDateTime = new UtcDateTime
                (
                    UtcDateTime.Year,
                    UtcDateTime.Month,
                    UtcDateTime.Day,
                    UtcDateTime.Hour,
                    UtcDateTime.Minute,
                    value,
                    UtcDateTime.Millisecond
                );

                if (!Equals(updatedDateTime, UtcDateTime))
                    UtcDateTime = updatedDateTime;
            }
        }

        public double HourScrollValue
        {
            get => _hourScrollValue;
            set
            {
                if (value < _hourScrollValue)
                    UtcDateTime += HourIncrement;
                else
                    UtcDateTime -= HourIncrement;
            }
        }

        public double MinuteScrollValue
        {
            get => _minuteScrollValue;
            set
            {
                if (value < _minuteScrollValue)
                    UtcDateTime += MinuteIncrement;
                else
                    UtcDateTime -= MinuteIncrement;
            }
        }

        public double SecondScrollValue
        {
            get => _secondScrollValue;
            set
            {
                if (value < _secondScrollValue)
                    UtcDateTime += SecondIncrement;
                else
                    UtcDateTime -= SecondIncrement;
            }
        }

        public TimeZoneViewModel TimeZone1 { get; }
        public TimeZoneViewModel TimeZone2 { get; }
        public TimeZoneViewModel TimeZone3 { get; }
        public TimeZoneViewModel TimeZone4 { get; }
        public TimeZoneViewModel TimeZone5 { get; }
        public TimeZoneViewModel TimeZone6 { get; }
        public TimeZoneViewModel TimeZone7 { get; }
        public TimeZoneViewModel TimeZone8 { get; }
        public TimeZoneViewModel TimeZone9 { get; }

        private void UpdateTimeZoneViewModels()
        {
            TimeZone1.UtcDateTime = UtcDateTime;
            TimeZone2.UtcDateTime = UtcDateTime;
            TimeZone3.UtcDateTime = UtcDateTime;
            TimeZone4.UtcDateTime = UtcDateTime;
            TimeZone5.UtcDateTime = UtcDateTime;
            TimeZone6.UtcDateTime = UtcDateTime;
            TimeZone7.UtcDateTime = UtcDateTime;
            TimeZone8.UtcDateTime = UtcDateTime;
            TimeZone9.UtcDateTime = UtcDateTime;
        }
    }
}
