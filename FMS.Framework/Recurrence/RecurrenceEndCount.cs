namespace FMS.Framework.Recurrence
{
    public class RecurrenceEndCount : RecurrenceEnd
    {
        public RecurrenceEndCount(int occurrences)
        {
            Occurrences = occurrences;
        }

        public int Occurrences { get; }
    }
}
