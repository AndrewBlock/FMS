using System.Runtime.InteropServices;

namespace FMS.Framework.Interop
{
    internal static class WinApi
    {
        [DllImport("Kernel32.dll", EntryPoint = "GetSystemTime", CallingConvention = CallingConvention.Winapi)]
        public static extern void GetSystemTime(out SystemTime lpSystemTime);
    }
}
