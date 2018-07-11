using System;
using System.Collections.Generic;
using System.Linq;
using FMS.Framework.Core;
using FMS.Framework.Exceptions;
using FMS.Framework.Recurrence.Rules;

namespace FMS.Framework.Recurrence
{
    public class RecurrencePattern : IRecurrencePattern
    {
        public RecurrencePattern(Date dateStart, IRule rule, RecurrenceEnd end)
        {
            if (dateStart == null)
                throw new ArgumentException(nameof(dateStart));

            if (rule == null)
                throw new ArgumentException(nameof(rule));

            ValidateRecurrenceEnd(end, dateStart);

            DateStart = dateStart;
            Rule = rule;
            End = end;
        }

        public Date DateStart { get; }
        public IRule Rule { get; }
        public RecurrenceEnd End { get; }

        public RecurrenceType RecurrenceType => Rule.RecurrenceType;

        private static readonly IReadOnlyList<Type> ValidEndTypes = new []
        {
            typeof(RecurrenceEndDate),
            typeof(RecurrenceEndCount),
            typeof(RecurrenceEndNone)
        };

        private static void ValidateRecurrenceEnd(RecurrenceEnd end, Date dateStart)
        {
            if (!IsValidEndType(end))
                throw new ArgumentException(nameof(end));

            if (end is RecurrenceEndDate endDate && dateStart > endDate.EndDate)
                throw new InvalidRangeException($"{nameof(dateStart)}->{nameof(end)}.{nameof(RecurrenceEndDate.EndDate)}");

            if (end is RecurrenceEndCount endCount && endCount.Occurrences < 1)
                throw new InvalidRangeException($"{nameof(end)}.{nameof(RecurrenceEndCount.Occurrences)}");
        }

        private static bool IsValidEndType(RecurrenceEnd end)
        {
            if (end == null)
                return true;

            var endType = end.GetType();
            return ValidEndTypes.Any(type => type.IsAssignableFrom(endType));
        }

        public IReadOnlyList<Date> ResolveOccurrences(Date rangeStart, Date rangeEnd)
        {
            if (rangeStart == null)
                throw new ArgumentException(nameof(rangeStart));

            if (rangeEnd == null)
                throw new ArgumentException(nameof(rangeEnd));

            if (rangeStart > rangeEnd)
                throw new InvalidRangeException($"{nameof(rangeStart)}->{nameof(rangeEnd)}");

            var occurrenceCount = 0;
            var results = new List<Date>();

            var iterator = Rule.GetIterator(DateStart);
            foreach (var referenceDate in iterator.ReferenceDates)
            {
                var resolvedDate = Rule.ResolveDate(referenceDate);

                if (resolvedDate == null)
                    continue;

                if (resolvedDate > rangeEnd)
                    break;

                if (End is RecurrenceEndDate endDate && resolvedDate > endDate.EndDate)
                    break;

                if (End is RecurrenceEndCount endCount && occurrenceCount >= endCount.Occurrences)
                    break;

                if (resolvedDate >= rangeStart)
                    results.Add(resolvedDate);

                occurrenceCount++;
            }

            return results;
        }
    }
}
