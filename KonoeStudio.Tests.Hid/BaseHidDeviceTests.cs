using NUnit.Framework;
using KonoeStudio.Libs.Hid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KonoeStudio.Tests.Hid.Mock;
using KonoeStudio.Tests.Hid.Stub;
using Microsoft.Win32.SafeHandles;

namespace KonoeStudio.Tests.Hid
{
    [TestFixture()]
    public class BaseHidDeviceTests
    {
        [Test()]
        public void BaseHidDeviceTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(BaseHidDeviceTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);

            mock.IsNotNull();
            mock.DisposeTokenSource.IsNotNull();
            mock.DisposeTokenSource.IsCancellationRequested.IsFalse();
            mock.DisposeTokenSource.Token.Is(mock.DisposeToken);
            mock.DisposeToken.IsCancellationRequested.IsFalse();

            mock.IsReadOpened.IsFalse();
            mock.IsWriteOpened.IsFalse();
            mock.DeviceInfo.Is(stubInfo);
        }

        [Test()]
        public void BaseHidDeviceExceptionTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(BaseHidDeviceExceptionTest)}", 1, 1);

            Assert.Catch<ArgumentNullException>(() => new MockHidDevice(null, stubHelper));
            Assert.Catch<ArgumentNullException>(() => new MockHidDevice(stubInfo, null));
            Assert.Catch<ArgumentNullException>(() => new MockHidDevice(null, null));
        }

        [TestCase(new byte[]{1, 2, 3, 4, 5})]
        public async Task ReadRawDataAsyncTest(byte[] data)
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadRawDataAsyncTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);
            mock.ReadHandle = new SafeFileHandle(new IntPtr(1), true);
            stubHelper.ReadReturnValue = data;
            stubHelper.IsReadAsync.IsFalse();

            var value = await mock.ReadRawDataAsync();
            value.SequenceEqual(data);
            stubHelper.IsReadAsync.IsTrue();
        }

        [Test()]
        public void ReadRawDataAsyncCancelTest()
        {
            var cts = new CancellationTokenSource();
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadRawDataAsyncCancelTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);
            mock.ReadHandle = new SafeFileHandle(new IntPtr(1), true);
            stubHelper.IsDelay = true;

            var value = mock.ReadRawDataAsync(cts.Token);
            cts.Cancel();
            Assert.CatchAsync<OperationCanceledException>(() => value);
        }

        [Test()]
        public void ReadRawDataAsyncExceptionTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadRawDataAsyncExceptionTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);

            Assert.CatchAsync<DeviceIsNotOpenedException>(() => mock.ReadRawDataAsync());
        }

        [Test()]
        public void ReadRawDataAsyncExceptionTest1()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadRawDataAsyncExceptionTest1)}", 0, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);
            mock.ReadHandle = new SafeFileHandle(new IntPtr(1), true);

            Assert.CatchAsync<HasNotCapabilityException>(() => mock.ReadRawDataAsync());
        }

        [TestCase(new byte[] {0x00, 0x01, 0x02})]
        public async Task WriteRawDataAsyncTest(byte[] data)
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadOpenTest)}", 1, 10);
            var mock = new MockHidDevice(stubInfo, stubHelper);
            mock.WriteHandle = new SafeFileHandle(new IntPtr(1), true);
            stubHelper.IsWriteAsync.IsFalse();

            await mock.WriteRawDataAsync(data);
            stubHelper.IsWriteAsync.IsTrue();
            stubHelper.WriteReturnValue.SequenceEqual(data);
        }

        [Test()]
        public void WriteRawDataAsyncCancelTest()
        {
            var cts = new CancellationTokenSource();
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadOpenTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);
            mock.WriteHandle = new SafeFileHandle(new IntPtr(1), true);
            stubHelper.IsDelay = true;

            var value = mock.WriteRawDataAsync(new byte[] {0x00}, cts.Token);
            cts.Cancel();
            Assert.CatchAsync<OperationCanceledException>(() => value);
        }

        [Test()]
        public void WriteRawDataAsyncExceptionTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadOpenTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);

            Assert.CatchAsync<ArgumentNullException>(() => mock.WriteRawDataAsync(null));
        }

        [Test()]
        public void WriteRawDataAsyncExceptionTest1()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadOpenTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);

            Assert.CatchAsync<DeviceIsNotOpenedException>(() => mock.WriteRawDataAsync(new byte[] { }));
        }

        [Test()]
        public void WriteRawDataAsyncExceptionTest2()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadOpenTest)}", 1, 0);
            var mock = new MockHidDevice(stubInfo, stubHelper);
            mock.WriteHandle = new SafeFileHandle(new IntPtr(1), true);

            Assert.CatchAsync<HasNotCapabilityException>(() => mock.WriteRawDataAsync(new byte[]{}));
        }

        [Test()]
        public void WriteRawDataAsyncExceptionTest3()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadOpenTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);
            mock.WriteHandle = new SafeFileHandle(new IntPtr(1), true);

            Assert.CatchAsync<HasNotCapabilityException>(() => mock.WriteRawDataAsync(new byte[2] {0x00, 0x00}));
        }

        [Test()]
        public void ReadOpenTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadOpenTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);

            mock.ReadOpen().IsTrue();
            mock.IsReadOpened.IsTrue();
        }

        [Test()]
        public void ReadOpenFailureTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadOpenFailureTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);
            
            stubHelper.IsFailureHandle = true;
            mock.WriteOpen().IsFalse();
            mock.IsWriteOpened.IsFalse();
        }

        [Test()]
        public void ReadOpenExceptionTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadOpenExceptionTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);

            stubHelper.IsThrow = true;
            Assert.Catch<DeviceCouldNotOpenedException>(() => mock.ReadOpen());
        }

        [Test()]
        public void WriteOpenTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(WriteOpenTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);

            mock.WriteOpen().IsTrue();
            mock.IsWriteOpened.IsTrue();
        }

        [Test()]
        public void WriteOpenFailureTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(WriteOpenFailureTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);

            stubHelper.IsFailureHandle = true;
            mock.WriteOpen().IsFalse();
            mock.IsWriteOpened.IsFalse();
        }

        [Test()]
        public void WriteOpenExceptionTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(WriteOpenExceptionTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);

            stubHelper.IsThrow = true;
            Assert.Catch<DeviceCouldNotOpenedException>(() => mock.WriteOpen());
        }

        [Test()]
        public void ReadCloseTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(ReadCloseTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);

            mock.ReadHandle = new SafeFileHandle(new IntPtr(1), true);
            mock.ReadClose();
            mock.IsReadOpened.IsFalse();
            mock.ReadHandle.IsClosed.IsTrue();
        }

        [Test()]
        public void WriteCloseTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(WriteCloseTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);
            
            mock.WriteHandle = new SafeFileHandle(new IntPtr(1), true);
            mock.WriteClose();
            mock.IsWriteOpened.IsFalse();
            mock.WriteHandle.IsClosed.IsTrue();
        }

        [Test()]
        public void DisposeTest()
        {
            var stubHelper = new StubNativeHelper();
            var stubInfo = new StubDeviceInfo($"{nameof(WriteCloseTest)}", 1, 1);
            var mock = new MockHidDevice(stubInfo, stubHelper);

            mock.ReadHandle = new SafeFileHandle(new IntPtr(1), true);
            mock.WriteHandle = new SafeFileHandle(new IntPtr(1), true);
            
            mock.Dispose();
            mock.IsReadOpened.IsFalse();
            mock.ReadHandle.IsClosed.IsTrue();
            mock.IsWriteOpened.IsFalse();
            mock.WriteHandle.IsClosed.IsTrue();
            mock.DisposeToken.IsCancellationRequested.IsTrue();
        }
    }
}