using KonoeStudio.Libs.Hid;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KonoeStudio.Demo.Hid
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //HidReport report = new HidReport(1, null);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var list = new HidDeviceInfoCollection(Encoding.GetEncoding(NativeMethods.GetConsoleOutputCP()));
            foreach (IHidDeviceInfo deviceInfo in list)
            {
                Console.WriteLine(deviceInfo.Description);
                Console.WriteLine(deviceInfo.DevicePath);
                Console.WriteLine($"VID  : {deviceInfo.Attributes.VendorID:x4}       PID: {deviceInfo.Attributes.ProductID:x4}");
                Console.WriteLine($"Usage: {deviceInfo.Capabilities.Usage:x4} UsagePage: {deviceInfo.Capabilities.UsagePage:x4}");
                Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------");
            }

            //var device = list.FirstOrDefault(deviceInfo => deviceInfo.DevicePath == @"");
            //var writeDevice = new HidDevice(@"\\?\hid#vid_17ef&pid_a018&mi_03&col02#7&26d71292&0&0001#{4d1e55b2-f16f-11cf-88cb-001111000030}"); // LED Change Write VID  : 17ef PID: a018 Usage: 0001 UsagePage: 000b
            //writeDevice.WriteOpen();
            //await writeDevice.WriteRawDataAsync(new byte[] { 2, 1 }, CancellationToken.None).ConfigureAwait(false);

            //var readDevice = new HidDevice(@"\\?\hid#vid_17ef&pid_60cf&col01#6&393de3ee&1&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}"); // Button Read VID  : 17ef       PID: 60cf Usage: 0001 UsagePage: ff01
            //readDevice.ReadOpen();
            //var tokenSource = new CancellationTokenSource();
            //var readData = await readDevice.ReadReportAsync(CancellationToken.None).ConfigureAwait(false);

            //var readTask = readDevice.ReadReportAsync(CancellationToken.None).ContinueWith(t =>
            //{
            //    if (t.IsFaulted)
            //    {
            //        Console.WriteLine("Faulted");
            //    }
            //    else if (t.IsCanceled)
            //    {
            //        Console.WriteLine("Canceled");
            //    }
            //    else if (t.IsCompleted)
            //    {
            //        Console.WriteLine($"Button: ReportId: {t.Result.ReportId}, data[0]: {t.Result.Data[0]}");
            //        tokenSource?.Cancel();
            //    }
            //}).ConfigureAwait(false);


            //var streamdevice = new HidDevice(@"\\?\hid#vid_17ef&pid_721f&mi_02#7&2767accc&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}"); // HDMI Ingest VID  : 17ef       PID: 721f Usage: 0001 UsagePage: ff00
            //streamdevice.ReadOpen();
            //await foreach( var item in streamdevice.GetReadReportStreamAsync().WithCancellation(tokenSource.Token))
            //{
            //    Console.WriteLine($"{DateTime.Now}: ReportId: {item.ReportId}, data[0]: {item.Data[0]}");
            //}

            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
        }
    }

    public static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern int GetConsoleOutputCP();
    }
}
