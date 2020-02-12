using System;

namespace KonoeStudio.Libs.Hid
{
    public class HidDeviceInfo : BaseHidDeviceInfo
    {
        public HidDeviceInfo(string devicePath, string description) : base(devicePath, description, new NativeHelper())
        {
            
        }
    }
}
