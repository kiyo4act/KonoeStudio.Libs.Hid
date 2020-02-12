using System;

namespace KonoeStudio.Libs.Hid
{
    public class HidDevice : BaseHidDevice
    {
        public HidDevice(string devicePath) : this(new HidDeviceInfo(devicePath, string.Empty))
        {
        }
        public HidDevice(IHidDeviceInfo deviceInfo) : base(deviceInfo, new NativeHelper())
        {
        }
    }
}
