namespace KonoeStudio.Libs.Hid
{
    public interface IHidDeviceInfo
    {
        string DevicePath { get; }
        string Description { get; }
        HidCapabilities Capabilities { get; }
        HidAttributes Attributes { get; }
    }
}
