using System.Text;

namespace KonoeStudio.Libs.Hid
{
    public class HidDeviceInfoCollection : BaseHidDeviceInfoCollection
    {
        public HidDeviceInfoCollection(Encoding? descriptionEncoding = null) : base(new NativeHelper(descriptionEncoding))
        {
        }
    }
}
