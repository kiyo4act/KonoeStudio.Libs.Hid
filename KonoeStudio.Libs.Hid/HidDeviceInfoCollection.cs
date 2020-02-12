namespace KonoeStudio.Libs.Hid
{
    public class HidDeviceInfoCollection : BaseHidDeviceInfoCollection
    {
        public HidDeviceInfoCollection() : base(new NativeHelper())
        {
        }
    }
}
