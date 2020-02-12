using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KonoeStudio.Libs.Hid;
using KonoeStudio.Tests.Hid.Mock;
using Microsoft.Win32.SafeHandles;

namespace KonoeStudio.Tests.Hid.Stub
{
    public class StubNativeHelper : INativeHelper
    {
        public HidAttributes GetAttributes(SafeFileHandle hidHandle)
        {
            return new HidAttributes();
        }

        public HidCapabilities GetCapabilities(SafeFileHandle hidHandle)
        {
            return new HidCapabilities();
        }

        public SafeFileHandle OpenDevice(string devicePath, DesiredAccesses deviceAccesses, ShareModes shareModes, FileFlags flags)
        {
            return new SafeFileHandle(IntPtr.Zero, true);
        }

        public IEnumerable<IHidDeviceInfo> EnumerateDeviceInfo()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new MockDeviceInfo($"devicePath{i}", $"description{i}", this);
            }
        }

        public Task<byte[]> ReadAsync(SafeFileHandle handle, short inputReportByteLength, CancellationToken token)
        {
            return Task.FromResult(new byte[]{1, 2, 3, 4, 5});
        }

        public Task WriteAsync(SafeFileHandle handle, short outputReportByteLength, byte[] data, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public bool ReleaseSafeDevInfoHandle(IntPtr handle)
        {
            return true;
        }

        public bool ReleasePreParsedHandle(IntPtr handle)
        {
            return true;
        }
    }
}