# KonoeStudio.Libs.Hid
Task-based asyncronouse read/write, async-streams (IAsyncEnumerable), and Enumerate devices feature for HID C# Library.
And this software is released under the MIT License, see [LICENSE](https://github.com/18konoe/KonoeStudio.Libs.DependencyInjection/blob/master/LICENSE)

You can download from [nuget](https://www.nuget.org/packages/KonoeStudio.Libs.Hid/) to use this library.
## Supported Framework
* .NET Standard 2.0

## For Japanese
Taskベースの非同期HIDライブラリです。C# 8.0から追加された非同期ストリーム(IAsyncEnumerable)もサポートしています。

使い方説明はブログに書き次第Updateします。

日本語OSの場合はデバイスのDescriptionが日本語になりますので、下記のように文字コードの指定が必要です。
```csharp
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var list = new HidDeviceInfoCollection(Encoding.GetEncoding("shift-jis"));
```

## Quick Tutorial
```csharp
// Create HID Device informations enumeration class instance
var list = new HidDeviceInfoCollection();

// Foreach HID Device information from list
foreach (IHidDeviceInfo deviceInfo in list)
{
    Console.WriteLine(deviceInfo.Description);
    Console.WriteLine(deviceInfo.DevicePath);
    Console.WriteLine($"VID  : {deviceInfo.Attributes.VendorID:x4}       PID: {deviceInfo.Attributes.ProductID:x4}");
    Console.WriteLine($"Usage: {deviceInfo.Capabilities.Usage:x4} UsagePage: {deviceInfo.Capabilities.UsagePage:x4}");
    Console.WriteLine("--------------------------------------------------------------------------------------------");
}

// GetDevice from Device information
var device1 = list.FirstOrDefault(info => info.Attributes.ProductID == 0x0000 && info.Attributes.VendorID == 0x0000)?.GetDevice();

// Or, create from Device Path
var device2 = new HidDevice(@"\\?\hid#vid_0000&pid_0000&mi_03&col02#7&26d71292&0&0001#{4d1e55b2-f16f-11cf-88cb-001111000030}");

// Open for read and await reading
device.ReadOpen();
var readData = await device.ReadReportAsync();

// Open for write and await writing
device.WriteOpen();
await device.WriteRawDataAsync(new byte[] { 2, 1 });
```
## Extra Tutorial
```csharp
// Read for async-stream
var streamdevice = new HidDevice(@"\\?\hid#vid_0000&pid_0000&mi_02#7&2767accc&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}");
streamdevice.ReadOpen();
await foreach (var item in streamdevice.GetReadReportStreamAsync().WithCancellation(tokenSource.Token))
{
    Console.WriteLine($"{DateTime.Now}: ReportId: {item.ReportId}, data[0]: {item.Data[0]}");
}
```