using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace KonoeStudio.Libs.Hid
{
    public static class HidExtendMethods
    {
        public static async Task<IHidReport> ReadReportAsync(this IHidDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException($"{nameof(device)} is null");
            }
            return new HidReport(await device.ReadRawDataAsync().ConfigureAwait(false), device.DeviceInfo.Capabilities.InputReportByteLength);
        }

        public static async Task<IHidReport> ReadReportAsync(this IHidDevice device, CancellationToken token)
        {
            if (device == null)
            {
                throw new ArgumentNullException($"{nameof(device)} is null");
            }
            return new HidReport(await device.ReadRawDataAsync(token).ConfigureAwait(false), device.DeviceInfo.Capabilities.InputReportByteLength);
        }

        public static Task WriteReportAsync(this IHidDevice device, IHidReport report)
        {
            if (device == null)
            {
                throw new ArgumentNullException($"{nameof(device)} is null");
            }

            return device.WriteRawDataAsync(report.GetWholeData());
        }

        public static Task WriteReportAsync(this IHidDevice device, IHidReport report, CancellationToken token)
        {
            if (device == null)
            {
                throw new ArgumentNullException($"{nameof(device)} is null");
            }
            if (report == null)
            {
                throw new ArgumentNullException($"{nameof(report)} is null");
            }
            return device.WriteRawDataAsync(report.GetWholeData(), token);
        }

        public static IHidDevice GetDevice(this IHidDeviceInfo deviceInfo)
        {
            return new HidDevice(deviceInfo);
        }

        public static async IAsyncEnumerable<IHidReport> GetReadReportStreamAsync(this IHidDevice device, [EnumeratorCancellation] CancellationToken token = default)
        {
            if (device == null)
            {
                throw new ArgumentNullException($"{nameof(device)} is null");
            }

            while (!token.IsCancellationRequested)
            {
                yield return await device.ReadReportAsync(token).ConfigureAwait(false);
            }
        }

        public static async Task WriteReportStreamAsync(this IHidDevice device, IAsyncEnumerable<IHidReport> stream, CancellationToken token = default)
        {
            if (device == null)
            {
                throw new ArgumentNullException($"{nameof(device)} is null");
            }
            if (stream == null)
            {
                throw new ArgumentNullException($"{nameof(stream)} is null");
            }
            await foreach (var item in stream.WithCancellation(token))
            {
                await device.WriteReportAsync(item, token).ConfigureAwait(false);
            }
        }
    }
}
