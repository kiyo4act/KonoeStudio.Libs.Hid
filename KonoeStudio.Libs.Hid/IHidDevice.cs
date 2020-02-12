using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace KonoeStudio.Libs.Hid
{
    public interface IHidDevice : IDisposable
    {
        IHidDeviceInfo DeviceInfo { get; }
        bool IsReadOpened { get; }
        bool IsWriteOpened { get; }
        Task<byte[]> ReadRawDataAsync();
        Task<byte[]> ReadRawDataAsync(CancellationToken token);
        Task WriteRawDataAsync(byte[] data);
        Task WriteRawDataAsync(byte[] data, CancellationToken token);
        bool ReadOpen();
        bool WriteOpen();
        void ReadClose();
        void WriteClose();
    }
}
