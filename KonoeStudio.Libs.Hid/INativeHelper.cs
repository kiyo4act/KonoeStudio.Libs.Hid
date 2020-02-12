using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace KonoeStudio.Libs.Hid
{
    public interface INativeHelper
    {
        HidAttributes GetAttributes(SafeFileHandle hidHandle);
        HidCapabilities GetCapabilities(SafeFileHandle hidHandle);
        SafeFileHandle OpenDevice(string devicePath, DesiredAccesses deviceAccesses, ShareModes shareModes, FileFlags flags);
        IEnumerable<IHidDeviceInfo> EnumerateDeviceInfo();
        Task<byte[]> ReadAsync(SafeFileHandle handle, short inputReportByteLength, CancellationToken token);
        Task WriteAsync(SafeFileHandle handle, short outputReportByteLength, byte[] data, CancellationToken token);
        bool ReleaseSafeDevInfoHandle(IntPtr handle);
        bool ReleasePreParsedHandle(IntPtr handle);
    }
}
