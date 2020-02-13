using System;
using System.Runtime.InteropServices;

namespace KonoeStudio.Libs.Hid
{
    public class SafePreParsedDataHandle : SafeHandle
    {
        public INativeHelper Helper { get; }

        public SafePreParsedDataHandle() : this(new NativeHelper())
        {
        }
        public SafePreParsedDataHandle(INativeHelper helper) : base(IntPtr.Zero, true)
        {
            Helper = helper ?? throw new ArgumentNullException($"{nameof(helper)} is null");
        }
        public SafePreParsedDataHandle(IntPtr existingHandle, INativeHelper helper) : base (existingHandle, true)
        {
            Helper = helper ?? throw new ArgumentNullException($"{nameof(helper)} is null");
        }

        protected override bool ReleaseHandle()
        {
            return Helper.ReleasePreParsedHandle(handle);
        }

        public override bool IsInvalid => handle == IntPtr.Zero;
    }
}
