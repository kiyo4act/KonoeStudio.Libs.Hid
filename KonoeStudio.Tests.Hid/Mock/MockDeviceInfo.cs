using KonoeStudio.Libs.Hid;

namespace KonoeStudio.Tests.Hid.Mock
{
    public class MockDeviceInfo : BaseHidDeviceInfo
    {
        public MockDeviceInfo(string devicePath, string description, INativeHelper helper) : base(devicePath, description, helper)
        {
        }
    }
}