using KonoeStudio.Libs.Hid;

namespace KonoeStudio.Tests.Hid.Mock
{
    public class MockDeviceInfoCollection : BaseHidDeviceInfoCollection
    {
        public MockDeviceInfoCollection(INativeHelper helper) : base(helper)
        {
        }
    }
}