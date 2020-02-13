using KonoeStudio.Libs.Hid;

namespace KonoeStudio.Tests.Hid.Stub
{
    public class StubDeviceInfo : IHidDeviceInfo
    {
        public string DevicePath { get; }
        public string Description { get; }
        public HidCapabilities Capabilities { get; }
        public HidAttributes Attributes { get; }

        public StubDeviceInfo(string devicePath, short inputReportByteLength, short outputReportByteLength)
        {
            DevicePath = devicePath;
            Capabilities = new HidCapabilities(0, 0, inputReportByteLength, outputReportByteLength, 0, new short[]{}, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }
    }
}