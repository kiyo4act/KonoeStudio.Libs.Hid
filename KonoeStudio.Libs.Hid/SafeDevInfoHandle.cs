using System;
using System.Runtime.InteropServices;

namespace KonoeStudio.Libs.Hid
{
    public class SafeDevInfoHandle : SafeHandle
    {
        public INativeHelper Helper { get; }
        public SafeDevInfoHandle() : this(new NativeHelper())
        {
        }

        public SafeDevInfoHandle(INativeHelper helper) : base(IntPtr.Zero, true)
        {
            Helper = helper ?? throw new ArgumentNullException($"{nameof(helper)} is null");
        }
        public SafeDevInfoHandle(IntPtr existingHandle, INativeHelper helper) : base(existingHandle, true)
        {
            Helper = helper ?? throw new ArgumentNullException($"{nameof(helper)} is null");
        }
        protected override bool ReleaseHandle()
        {
            return Helper.ReleaseSafeDevInfoHandle(handle);
        }

        public override bool IsInvalid => handle == IntPtr.Zero;
    }
}
