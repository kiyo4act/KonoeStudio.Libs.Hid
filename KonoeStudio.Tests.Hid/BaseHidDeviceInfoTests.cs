using NUnit.Framework;
using KonoeStudio.Libs.Hid;
using System;
using System.Collections.Generic;
using System.Text;
using KonoeStudio.Tests.Hid.Mock;
using KonoeStudio.Tests.Hid.Stub;

namespace KonoeStudio.Tests.Hid
{
    [TestFixture()]
    public class BaseHidDeviceInfoTests
    {
        [TestCase("TestPath", "TestDescription")]
        public void BaseHidDeviceInfoTest(string path, string description)
        {
            var stub = new StubNativeHelper();
            var mock = new MockDeviceInfo(path, description, stub);
            mock.IsNotNull();
            mock.DevicePath.Is(path);
            mock.Description.Is(description);

            mock.Capabilities.IsNotNull();
            stub.IsGetCapabilities.IsTrue();

            mock.Attributes.IsNotNull();
            stub.IsGetAttributes.IsTrue();
        }
    }
}