using KonoeStudio.Libs.Hid;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KonoeStudio.Demo.Hid
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //HidReport report = new HidReport(1, null);

            var list = new HidDeviceInfoCollection();
            foreach (IHidDeviceInfo deviceInfo in list)
            {
                Console.WriteLine(deviceInfo.DevicePath);
                Console.WriteLine(deviceInfo.Description);
                Console.WriteLine(deviceInfo.Attributes.VendorID);
                Console.WriteLine(deviceInfo.Attributes.ProductID);
                Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------");
            }

            //var device = list.FirstOrDefault(deviceInfo => deviceInfo.DevicePath == @"");
            var device = new HidDevice(@"\\?\hid#vid_046d&pid_c52b&mi_00#8&2be01977&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}");
            //var d = device?.GetDevice();
            var r = device.ReadOpen();
            var data = await device.ReadRawDataAsync(CancellationToken.None).ConfigureAwait(false);

            Console.ReadKey();
        }
    }
}
