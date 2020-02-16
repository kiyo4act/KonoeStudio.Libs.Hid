using NUnit.Framework;
using KonoeStudio.Libs.Hid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KonoeStudio.Tests.Hid.Mock;
using KonoeStudio.Tests.Hid.Stub;

namespace KonoeStudio.Tests.Hid
{
    [TestFixture()]
    public class BaseHidDeviceInfoCollectionTests
    {
        public IEnumerable<IHidDeviceInfo> TestInfoData()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new MockDeviceInfo($"devicePath{i}", $"description{i}", new StubNativeHelper());
            }
        }

        [Test()]
        public void GetEnumeratorTest()
        {
            var stub = new StubNativeHelper();
            stub.EnumerateDeviceInfoReturnValue = TestInfoData();
            MockDeviceInfoCollection mock = new MockDeviceInfoCollection(stub);

            mock.IsNotNull();
            mock.SequenceEqual(TestInfoData());
        }

        [Test()]
        public void GetEnumeratorTest1()
        {
            var stub = new StubNativeHelper();
            MockDeviceInfoCollection mock = new MockDeviceInfoCollection(stub);

            mock.IsNotNull();
            mock.Count.Is(0);
        }

        [Test()]
        public void GetEnumeratorExceptionTest()
        {
            Assert.Catch<ArgumentNullException>(() => new MockDeviceInfoCollection(null));
        }
    }
}