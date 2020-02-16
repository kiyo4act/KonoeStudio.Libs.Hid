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
        public bool IsReleasePreParsedHandle { get; private set; } = false;
        public bool IsReleaseSafeDevInfoHandle { get; private set; } = false;
        public bool IsGetAttributes { get; private set; } = false;
        public bool IsGetCapabilities { get; private set; } = false;
        public bool IsOpenDevice { get; private set; } = false;
        public bool IsReadAsync { get; private set; } = false;
        public bool IsWriteAsync { get; private set; } = false;
        public bool IsThrow { get; set; } = false;
        public bool IsFailureHandle { get; set; } = false;
        public bool IsDelay { get; set; } = false;
        public byte[] ReadReturnValue { get; set; } = {};
        public byte[] WriteReturnValue { get; set; } = null;
        public IEnumerable<IHidDeviceInfo> EnumerateDeviceInfoReturnValue { get; set; }

    public HidAttributes GetAttributes(SafeFileHandle hidHandle)
        {
            IsGetAttributes = true;
            return new HidAttributes();
        }

        public HidCapabilities GetCapabilities(SafeFileHandle hidHandle)
        {
            IsGetCapabilities = true;
            return new HidCapabilities();
        }

        public SafeFileHandle OpenDevice(string devicePath, DesiredAccesses deviceAccesses, ShareModes shareModes, FileFlags flags)
        {
            if(IsThrow) throw new Exception("TestException");
            var returnValue = IsFailureHandle ? IntPtr.Zero : new IntPtr(1);
            IsOpenDevice = true;
            return new SafeFileHandle(returnValue, true);
        }

        public IEnumerable<IHidDeviceInfo> EnumerateDeviceInfo()
        {
            return EnumerateDeviceInfoReturnValue;
        }

        public async Task<byte[]> ReadAsync(SafeFileHandle handle, short inputReportByteLength, CancellationToken token)
        {
            if (IsDelay) await Task.Delay(5000, token).ConfigureAwait(false);
            IsReadAsync = true;
            return ReadReturnValue;
        }

        public async Task WriteAsync(SafeFileHandle handle, short outputReportByteLength, byte[] data, CancellationToken token)
        {
            if (IsDelay) await Task.Delay(50000, token).ConfigureAwait(false);
            IsWriteAsync = true;
            WriteReturnValue = data;
        }

        public bool ReleaseSafeDevInfoHandle(IntPtr handle)
        {
            IsReleaseSafeDevInfoHandle = true;
            return true;
        }

        public bool ReleasePreParsedHandle(IntPtr handle)
        {
            IsReleasePreParsedHandle = true;
            return true;
        }
    }
}