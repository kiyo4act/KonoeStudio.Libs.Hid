using System;
using System.Runtime.InteropServices;

namespace KonoeStudio.Libs.Hid
{
    [StructLayout(LayoutKind.Sequential)]
    internal class SP_DEVINFO_DATA
    {
        internal int cbSize = Marshal.SizeOf<SP_DEVINFO_DATA>();
        internal Guid ClassGuid = Guid.Empty;
        internal int DevInst = 0;
        internal IntPtr Reserved = IntPtr.Zero;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class SP_DEVICE_INTERFACE_DATA
    {
        internal int cbSize = Marshal.SizeOf<SP_DEVICE_INTERFACE_DATA>();
        internal Guid InterfaceClassGuid;
        internal int Flags;
        internal IntPtr Reserved = IntPtr.Zero;
    }

    // TODO: Support for dynamic length because there is no reason to set size=255
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal class SP_DEVICE_INTERFACE_DETAIL_DATA
    {
        internal int cbSize = IntPtr.Size == 4 ? 4 + Marshal.SystemDefaultCharSize : 8;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        internal string? DevicePath;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class DEVPROPKEY
    {
        internal Guid fmtid = new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2);
        internal ulong pid = 4;
    }

    // HACK: Investigate to work even if define class instead of struct
    [StructLayout(LayoutKind.Sequential)]
    internal struct SECURITY_ATTRIBUTES
    {
        internal int nLength;
        internal IntPtr lpSecurityDescriptor;
        internal bool bInheritHandle;

        internal SECURITY_ATTRIBUTES(bool b)
        {
            //nLength = Marshal.SizeOf<SECURITY_ATTRIBUTES>();
            nLength = Marshal.SizeOf(default(SECURITY_ATTRIBUTES));
            lpSecurityDescriptor = IntPtr.Zero;
            bInheritHandle = b;
        }
    }
}
