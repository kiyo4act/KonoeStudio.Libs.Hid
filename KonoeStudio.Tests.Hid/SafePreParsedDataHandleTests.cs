using System;
using NUnit.Framework;
using KonoeStudio.Libs.Hid;
using KonoeStudio.Tests.Hid.Stub;

namespace KonoeStudio.Tests.Hid
{
    [TestFixture()]
    public class SafePreParsedDataHandleTests
    {
        [Test()]
        public void SafePreParsedDataHandleTest()
        {
            var target = new SafePreParsedDataHandle();
            target.IsNotNull();
            target.Helper.IsInstanceOf<INativeHelper>();
            target.DangerousGetHandle().Is(IntPtr.Zero);
            target.IsInvalid.IsTrue();
            target.IsClosed.IsFalse();
        }

        [Test()]
        public void SafePreParsedDataHandleTest1()
        {
            var stub = new StubNativeHelper();
            var target = new SafePreParsedDataHandle(stub);
            target.IsNotNull();
            target.Helper.Is(stub);
            target.DangerousGetHandle().Is(IntPtr.Zero);
            target.IsInvalid.IsTrue();
            target.IsClosed.IsFalse();
        }

        [Test()]
        public void SafePreParsedDataHandleTest2()
        {
            IntPtr ptr = new IntPtr(1);
            var stub = new StubNativeHelper();
            var target = new SafePreParsedDataHandle(ptr, stub);
            target.IsNotNull();
            target.Helper.Is(stub);
            target.DangerousGetHandle().Is(ptr);
            target.IsInvalid.IsFalse();
            target.IsClosed.IsFalse();
        }

        [Test()]
        public void DisposeTest()
        {
            IntPtr ptr = new IntPtr(1);
            var stub = new StubNativeHelper();
            var target = new SafePreParsedDataHandle(ptr, stub);
            stub.IsReleasePreParsedHandle.IsFalse();

            target.Dispose();
            target.IsClosed.IsTrue();
            stub.IsReleasePreParsedHandle.IsTrue();
        }
    }
}