using KonoeStudio.Libs.Hid;

namespace KonoeStudio.Tests.Hid.Mock
{
    public class MockHidDevice : BaseHidDevice
    {
        public MockHidDevice(IHidDeviceInfo deviceInfo, INativeHelper helper) : base(deviceInfo, helper)
        {
        }
    }
}