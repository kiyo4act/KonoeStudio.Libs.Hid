using System.Threading;
using KonoeStudio.Libs.Hid;
using Microsoft.Win32.SafeHandles;

namespace KonoeStudio.Tests.Hid.Mock
{
    public class MockHidDevice : BaseHidDevice
    {
        public new SafeFileHandle ReadHandle
        {
            get => base.ReadHandle;
            set => base.ReadHandle = value;
        }

        public new SafeFileHandle WriteHandle
        {
            get => base.WriteHandle;
            set => base.WriteHandle = value;
        }

        public new CancellationTokenSource DisposeTokenSource
        {
            get => base.DisposeTokenSource;
            set => base.DisposeTokenSource = value;
        }
        public new CancellationToken DisposeToken
        {
            get => base.DisposeToken;
            set => base.DisposeToken = value;
        }

        public MockHidDevice(IHidDeviceInfo deviceInfo, INativeHelper helper) : base(deviceInfo, helper)
        {
        }
    }
}