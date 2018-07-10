using System.Runtime.InteropServices;

namespace FMS.Framework.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Tzi
    {
        public int Bias;
        public int StandardBias;
        public int DaylightBias;
        public SystemTime StandardDate;
        public SystemTime DaylightDate;
    }
}
