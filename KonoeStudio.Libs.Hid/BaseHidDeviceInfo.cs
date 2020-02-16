using System;

namespace KonoeStudio.Libs.Hid
{
    public abstract class BaseHidDeviceInfo : IHidDeviceInfo
    {
        public string DevicePath { get; }
        public string Description { get; }
        public HidCapabilities Capabilities { get; }
        public HidAttributes Attributes { get; }
        protected INativeHelper Helper { get; }

        protected BaseHidDeviceInfo(string devicePath, string description, INativeHelper helper)
        {
            DevicePath = devicePath ?? throw new ArgumentNullException($"{nameof(devicePath)} is null");
            Description = description;
            Helper = helper ?? throw new ArgumentNullException($"{nameof(helper)} is null");

            // Just open to get capabilities and attributes
            using (var hidHandle = Helper.OpenDevice(DevicePath, DesiredAccesses.AccessNone,
                ShareModes.FileShareRead | ShareModes.FileShareWrite, FileFlags.FileFlagNone))
            {
                Capabilities = helper.GetCapabilities(hidHandle);
                Attributes = helper.GetAttributes(hidHandle);
            }
        }
    }
}
