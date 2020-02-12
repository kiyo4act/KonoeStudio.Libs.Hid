using System;
using System.Runtime.InteropServices;

namespace KonoeStudio.Libs.Hid
{
	[StructLayout(LayoutKind.Sequential)]
    public class HidAttributes
    {
        private readonly int _size;
        private readonly ushort _vendorId;
        private readonly ushort _productId;
        private readonly short _versionNumber;

        public int Size => _size;
        public ushort VendorID => _vendorId;
        public ushort ProductID => _productId;
        public short VersionNumber => _versionNumber;

        public HidAttributes()
        {
            _size = Marshal.SizeOf<HidAttributes>();
            _vendorId = default;
            _productId = default;
            _versionNumber = default;
        }
        public HidAttributes(int size, ushort vendorId, ushort productId, short versionNumber)
        {
            _size = size;
            _vendorId = vendorId;
            _productId = productId;
            _versionNumber = versionNumber;
        }
    }
}
