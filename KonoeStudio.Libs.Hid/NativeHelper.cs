using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace KonoeStudio.Libs.Hid
{
    internal class NativeHelper : INativeHelper
    {
        public Encoding DescriptionEncoding { get; set; }
        private Guid _hidClassGuid = Guid.Empty;
        public Guid HidClassGuid
        {
            get
            {
                if (_hidClassGuid == Guid.Empty)
                {
                    NativeMethods.HidD_GetHidGuid(out Guid hidClassGuid);
                    _hidClassGuid = hidClassGuid;
                }
                return _hidClassGuid;
            }
        }

        public NativeHelper(Encoding? descriptionEncoding = null)
        {
            DescriptionEncoding = descriptionEncoding ?? Encoding.Default;
        }
        public HidAttributes GetAttributes(SafeFileHandle handle)
        {
            if (handle == null)
            {
                throw new ArgumentNullException($"{nameof(handle)} is null");
            }

            var deviceAttributes = new HidAttributes();
            var result = NativeMethods.HidD_GetAttributes(handle, deviceAttributes);
            
            if (!result)
            {
                throw new GetHidAttributesException($"Could not get attribute by this {nameof(handle)}: {handle}");
            }

            return deviceAttributes;
        }

        public HidCapabilities GetCapabilities(SafeFileHandle handle)
        {
            if (handle == null)
            {
                throw new ArgumentNullException($"{nameof(handle)} is null");
            }

            IntPtr preparsedData = IntPtr.Zero;
            HidCapabilities capabilities = new HidCapabilities();

            var preParseResult = NativeMethods.HidD_GetPreparsedData(handle, ref preparsedData);

            if (!preParseResult)
            {
                throw new GetHidCapabilitiesException($"Could not get pre-parsed data by this {nameof(handle)}: {handle}");
            }

            bool isGetCaps = false;
            //HIDP_CAPS caps = new HIDP_CAPS();
            using (SafePreParsedDataHandle preParsedDataHandle = new SafePreParsedDataHandle(preparsedData, this))
            {
                var getCapResult = NativeMethods.HidP_GetCaps(preParsedDataHandle, capabilities);

                // Return value is HIDP_STATUS_SUCCESS or HIDP_STATUS_INVALID_PREPARSED_DATA
                // https://docs.microsoft.com/windows-hardware/drivers/ddi/hidpi/nf-hidpi-hidp_getbuttoncaps
                isGetCaps = getCapResult == NativeMethods.HIDP_STATUS.SUCCESS;
            }

            if (!isGetCaps)
            {
                throw new GetHidCapabilitiesException($"Could not get capabilities by this {nameof(handle)}: {handle}");
            }

            return capabilities;
        }

        public SafeFileHandle OpenDevice(string devicePath, DesiredAccesses deviceAccesses, ShareModes shareModes, FileFlags flags)
        {
            if (devicePath == null)
            {
                throw new ArgumentNullException($"{nameof(devicePath)} is null");
            }

            var security = new SECURITY_ATTRIBUTES(true);

            SafeFileHandle result = NativeMethods.CreateFile(devicePath, deviceAccesses, shareModes, ref security, NativeMethods.OPEN_EXISTING, flags, 0);

            if (result.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            return result;
        }

        public IEnumerable<IHidDeviceInfo> EnumerateDeviceInfo()
        {
            var hidClass = HidClassGuid;

            using SafeDevInfoHandle deviceInfoSet = NativeMethods.SetupDiGetClassDevs(hidClass, null, IntPtr.Zero, NativeMethods.DIGCF_PRESENT | NativeMethods.DIGCF_DEVICEINTERFACE);
            if (deviceInfoSet.IsInvalid)
            {
                yield break;
            }

            SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA();
            int deviceIndex = 0;

            while (NativeMethods.SetupDiEnumDeviceInfo(deviceInfoSet, deviceIndex, deviceInfoData))
            {
                deviceIndex += 1;

                SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
                int deviceInterfaceIndex = 0;

                while (NativeMethods.SetupDiEnumDeviceInterfaces(deviceInfoSet, deviceInfoData, hidClass, deviceInterfaceIndex, deviceInterfaceData))
                {
                    deviceInterfaceIndex++;
                    string devicePath = GetDevicePath(deviceInfoSet, deviceInterfaceData);
                    string busReportedDeviceDescription = GetBusReportedDeviceDescription(deviceInfoSet, deviceInfoData);
                    string deviceDescription = GetDeviceDescription(deviceInfoSet, deviceInfoData);
                    string description = string.IsNullOrEmpty(busReportedDeviceDescription) ? deviceDescription : busReportedDeviceDescription;
                    IHidDeviceInfo? info = null;
                    try
                    {
                        info = new HidDeviceInfo(devicePath, description);
                    }
                    catch (GetHidAttributesException)
                    {
                        //Console.WriteLine("GetHidCapabilitiesException");
                    }
                    catch (GetHidCapabilitiesException) { }

                    if (info != null)
                    {
                        yield return info;
                    }
                }
            }
        }

        public async Task<byte[]> ReadAsync(SafeFileHandle handle, short inputReportByteLength, CancellationToken token)
        {
            if (handle == null)
            {
                throw new ArgumentNullException($"{nameof(handle)} is null");
            }

            IntPtr unmanagedBuffer = IntPtr.Zero;
            byte[] result = new byte[inputReportByteLength];

            token.ThrowIfCancellationRequested();
            using (var resetEvent = new ManualResetEvent(false))
            {
                var overlapped = new NativeOverlapped
                {
                    OffsetLow = 0,
                    OffsetHigh = 0,
                    EventHandle = resetEvent.SafeWaitHandle.DangerousGetHandle()
                };

                try
                {
                    unmanagedBuffer = Marshal.AllocHGlobal(result.Length);
                    bool readResult = NativeMethods.ReadFile(handle, unmanagedBuffer, (uint)result.Length, out uint bytesRead, ref overlapped);

                    int hResult = Marshal.GetHRForLastWin32Error();
                    if (readResult || hResult == NativeMethods.ERROR_SUCCESS)
                    {
                        if (bytesRead != result.Length)
                        {
                            // TODO: Error handling if need
                        }
                        // Already I/O Completed
                        Marshal.Copy(unmanagedBuffer, result, 0, (int)bytesRead);
                        return result;
                    }
                    else if (hResult != NativeMethods.ERROR_IO_PENDING)
                    {
                        // Read error by other reason
                        Marshal.ThrowExceptionForHR(hResult);
                    }
                    else
                    {
                        bool isCanceled = false;
                        bool isComplete = false;
                        try
                        {
                            // Asynchronous waiting
                            isComplete = await resetEvent.WaitOneAsync(token).ConfigureAwait(false);
                        }
                        catch (OperationCanceledException)
                        {
                            isCanceled = true;
                        }

                        if (!isCanceled && isComplete)
                        {
                            // Asynchronous I/O Completed
                            bool overlappedResult = NativeMethods.GetOverlappedResult(handle, ref overlapped, out uint overlappedBytesTransferred, false);
                            if (!overlappedResult)
                            {
                                //int hResult = Marshal.GetHRForLastWin32Error();
                                //if (hResult != NativeMethods.ERROR_IO_PENDING)
                                // Maybe impossible if it is working fine
                                Marshal.ThrowExceptionForHR(hResult);
                            }
                            else if (overlappedBytesTransferred != result.Length)
                            {
                                // TODO: Error handling if need
                            }
                            Marshal.Copy(unmanagedBuffer, result, 0, (int)overlappedBytesTransferred);
                        }
                        else
                        {
                            // Need to cancel Asynchronous I/O
                            bool cancelResult = NativeMethods.CancelIoEx(handle, ref overlapped);
                            if (!cancelResult)
                            {
                                // TODO: Canceled handling if need
                                // https://docs.microsoft.com/en-us/windows/win32/fileio/canceling-pending-i-o-operations
                                //int hResult = Marshal.GetHRForLastWin32Error();
                                //if (hResult != NativeMethods.ERROR_NOT_FOUND)
                            }
                            token.ThrowIfCancellationRequested();
                        }
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(unmanagedBuffer);
                }
            }
            return result;
        }

        public async Task WriteAsync(SafeFileHandle handle, short outputReportByteLength, byte[] data, CancellationToken token)
        {
            if (handle == null)
            {
                throw new ArgumentNullException($"{nameof(handle)} is null");
            }
            if (data == null)
            {
                throw new ArgumentNullException($"{nameof(data)} is null");
            }

            token.ThrowIfCancellationRequested();
            using (var resetEvent = new ManualResetEvent(false))
            {
                var overlapped = new NativeOverlapped
                {
                    OffsetLow = 0,
                    OffsetHigh = 0,
                    EventHandle = resetEvent.SafeWaitHandle.DangerousGetHandle()
                };

                bool writeResult = NativeMethods.WriteFile(handle, data, (uint)data.Length, out uint bytesWritten, ref overlapped);

                int hResult = Marshal.GetHRForLastWin32Error();
                if (writeResult || hResult == NativeMethods.ERROR_SUCCESS)
                {
                    if (bytesWritten != data.Length)
                    {
                        // TODO: Error handling if need
                    }
                    // Already I/O Completed
                    return;
                }
                else if (hResult != NativeMethods.ERROR_IO_PENDING)
                {
                    // Read error by other reason
                    Marshal.ThrowExceptionForHR(hResult);
                }
                else
                {
                    bool isCanceled = false;
                    bool isComplete = false;
                    try
                    {
                        // Asynchronous waiting
                        isComplete = await resetEvent.WaitOneAsync(token).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        isCanceled = true;
                    }

                    if (!isCanceled && isComplete)
                    {
                        // Asynchronous I/O Completed
                        bool overlappedResult = NativeMethods.GetOverlappedResult(handle, ref overlapped, out uint overlappedBytesTransferred, false);
                        if (!overlappedResult)
                        {
                            //int hResult = Marshal.GetHRForLastWin32Error();
                            //if (hResult != NativeMethods.ERROR_IO_PENDING)
                            // Maybe impossible if it is working fine
                            Marshal.ThrowExceptionForHR(hResult);
                        }
                        else if (overlappedBytesTransferred != data.Length)
                        {
                            // TODO: Error handling if need
                        }
                    }
                    else
                    {
                        // Need to cancel Asynchronous I/O
                        bool cancelResult = NativeMethods.CancelIoEx(handle, ref overlapped);
                        if (!cancelResult)
                        {
                            // TODO: Canceled handling if need
                            // https://docs.microsoft.com/en-us/windows/win32/fileio/canceling-pending-i-o-operations
                            //int hResult = Marshal.GetHRForLastWin32Error();
                            //if (hResult != NativeMethods.ERROR_NOT_FOUND)
                        }
                        token.ThrowIfCancellationRequested();
                    }
                }
            }
        }

        public bool ReleaseSafeDevInfoHandle(IntPtr handle)
        {
            return NativeMethods.SetupDiDestroyDeviceInfoList(handle);
        }

        public bool ReleasePreParsedHandle(IntPtr handle)
        {
            return NativeMethods.HidD_FreePreparsedData(handle);
        }

        private string GetDevicePath(SafeDevInfoHandle deviceInfoSet, SP_DEVICE_INTERFACE_DATA deviceInterfaceData)
        {
            var bufferSize = 0;
            var interfaceDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();

            NativeMethods.SetupDiGetDeviceInterfaceDetailBuffer(deviceInfoSet, deviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, IntPtr.Zero);

            bool result = NativeMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, deviceInterfaceData,
                interfaceDetail, bufferSize, ref bufferSize, IntPtr.Zero);

            if (result && interfaceDetail.DevicePath != null)
            {
                return interfaceDetail.DevicePath;
            }

            return string.Empty;
        }

        private string GetDeviceDescription(SafeDevInfoHandle deviceInfoSet, SP_DEVINFO_DATA devinfoData)
        {
            var descriptionBuffer = new byte[1024];
            string resultString = string.Empty;
            var requiredSize = 0;
            var type = 0;

            var result = NativeMethods.SetupDiGetDeviceRegistryProperty(
                deviceInfoSet,
                devinfoData,
                NativeMethods.SPDRP_DEVICEDESC,
                ref type,
                descriptionBuffer,
                descriptionBuffer.Length,
                ref requiredSize);

            if (result)
            {
                Encoding.GetEncodings();
                resultString = DescriptionEncoding.GetString(descriptionBuffer).TrimEnd('\0');
            }

            return resultString;
        }

        private string GetBusReportedDeviceDescription(SafeDevInfoHandle deviceInfoSet, SP_DEVINFO_DATA devinfoData)
        {
            var descriptionBuffer = new byte[1024];
            string resultString = string.Empty;

            if (Environment.OSVersion.Version.Major > 5)
            {
                ulong propertyType = 0;
                var requiredSize = 0;
                var devPropKey = new DEVPROPKEY();
                var result = NativeMethods.SetupDiGetDeviceProperty(
                    deviceInfoSet,
                    devinfoData,
                    devPropKey,
                    ref propertyType,
                    descriptionBuffer,
                    descriptionBuffer.Length,
                    ref requiredSize,
                    0);

                if (result)
                {
                    resultString = DescriptionEncoding.GetString(descriptionBuffer).TrimEnd('\0');
                }
            }
            return resultString;
        }
    }
}
