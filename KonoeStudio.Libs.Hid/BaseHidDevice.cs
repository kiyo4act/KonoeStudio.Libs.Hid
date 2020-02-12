using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace KonoeStudio.Libs.Hid
{
    public abstract class BaseHidDevice : IHidDevice
    {
        public IHidDeviceInfo DeviceInfo { get; }
        public bool IsReadOpened => ReadHandle != null && !ReadHandle.IsClosed && !ReadHandle.IsInvalid;
        public bool IsWriteOpened => WriteHandle != null && !WriteHandle.IsClosed && !WriteHandle.IsInvalid;
        protected SafeFileHandle ReadHandle { get; private set; }
        protected SafeFileHandle WriteHandle { get; private set; }
        protected INativeHelper Helper { get; }
        protected CancellationTokenSource DisposeTokenSource { get; private set; } = new CancellationTokenSource();
        protected CancellationToken DisposeToken { get; }

        protected BaseHidDevice(IHidDeviceInfo deviceInfo, INativeHelper helper)
        {
            DeviceInfo = deviceInfo ?? throw new ArgumentNullException($"{nameof(deviceInfo)} is null");
            Helper = helper ?? throw new ArgumentNullException($"{nameof(helper)} is null");
            DisposeToken = DisposeTokenSource.Token;
            ReadHandle = new SafeFileHandle(IntPtr.Zero, true);
            WriteHandle = new SafeFileHandle(IntPtr.Zero, true);
        }

        public Task<byte[]> ReadRawDataAsync()
        {
            return ReadRawDataAsync(CancellationToken.None);
        }

        public async Task<byte[]> ReadRawDataAsync(CancellationToken token)
        {
            if (!IsReadOpened)
            {
                throw new DeviceIsNotOpenedException($"Invoke {nameof(ReadOpen)} method first. Current {nameof(IsReadOpened)}: {IsReadOpened}");
            }

            short inputReportByteLength = DeviceInfo.Capabilities.InputReportByteLength;

            if (inputReportByteLength <= 0)
            {
                throw new HasNotCapabilityException($"{nameof(DeviceInfo.Capabilities.InputReportByteLength)} is expected > 0. Actual: {inputReportByteLength}");
            }

            using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, DisposeToken))
            {
                return await Helper.ReadAsync(ReadHandle, inputReportByteLength, linkedTokenSource.Token).ConfigureAwait(false);
            }
        }

        public Task WriteRawDataAsync(byte[] data)
        {
            return WriteRawDataAsync(data, CancellationToken.None);
        }

        public async Task WriteRawDataAsync(byte[] data, CancellationToken token)
        {
            if (data == null)
            {
                throw new ArgumentNullException($"{nameof(data)} is null");
            }

            if (!IsWriteOpened)
            {
                throw new DeviceIsNotOpenedException($"Invoke {nameof(WriteOpen)} method first. Current {nameof(IsWriteOpened)}: {IsWriteOpened}");
            }

            short outputReportByteLength = DeviceInfo.Capabilities.OutputReportByteLength;

            if (outputReportByteLength <= 0)
            {
                throw new HasNotCapabilityException($"{nameof(DeviceInfo.Capabilities.OutputReportByteLength)} is expected > 0. Actual: {outputReportByteLength}");
            }

            if (data.Length > outputReportByteLength)
            {
                throw new HasNotCapabilityException($"{nameof(data)} length is too long. Expected: {outputReportByteLength}, Actual: {data.Length}");
            }

            using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, DisposeToken))
            {
                await Helper.WriteAsync(WriteHandle, outputReportByteLength, data, linkedTokenSource.Token).ConfigureAwait(false);
            }
        }

        public bool ReadOpen()
        {
            bool result = false;
            
            if (!IsReadOpened)
            {
                try
                {
                    ReadHandle.Dispose();
                    ReadHandle = Helper.OpenDevice(DeviceInfo.DevicePath, DesiredAccesses.GenericRead,
                        ShareModes.FileShareRead | ShareModes.FileShareWrite, FileFlags.FileFlagNone);
                    result = true;
                }
                catch (Exception e)
                {
                    throw new DeviceCouldNotOpenedException($"This device could not {nameof(ReadOpen)}. Please check inner exception ({e.GetType()})", e);
                }
            }

            return result;
        }

        public bool WriteOpen()
        {
            bool result = false;

            if (!IsWriteOpened)
            {
                try
                {
                    WriteHandle.Dispose();
                    WriteHandle = Helper.OpenDevice(DeviceInfo.DevicePath, DesiredAccesses.GenericWrite,
                        ShareModes.FileShareRead | ShareModes.FileShareWrite, FileFlags.FileFlagNone);
                    result = true;
                }
                catch (Exception e)
                {
                    throw new DeviceCouldNotOpenedException($"This device could not {nameof(WriteOpen)}. Please check inner exception ({e.GetType()})", e);
                }
            }

            return result;
        }

        public void ReadClose()
        {
            ReadHandle?.Dispose();
        }

        public void WriteClose()
        {
            WriteHandle?.Dispose();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeTokenSource?.Cancel();
                    DisposeTokenSource?.Dispose();

                    ReadHandle?.Dispose();
                    WriteHandle?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
