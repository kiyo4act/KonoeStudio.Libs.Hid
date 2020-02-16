using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace KonoeStudio.Libs.Hid
{
    internal static class NativeMethods
    {
        #region Constants

        internal const int ERROR_SUCCESS = -2147024896; // 0x80070000
        internal const int ERROR_IO_PENDING = -2147023899; // 0x800703E5
        //internal const int ERROR_NOT_FOUND = 1168; // 0x00000490
        internal const short OPEN_EXISTING = 3;
        internal const short DIGCF_PRESENT = 0x0002;
        internal const short DIGCF_DEVICEINTERFACE = 0x0010;
        internal const int SPDRP_DEVICEDESC = 0;

        #endregion

        #region I/O handling

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern SafeFileHandle CreateFile(
            string lpFileName,
            DesiredAccesses dwDesiredAccess,
            ShareModes dwShareMode,
            ref SECURITY_ATTRIBUTES lpSecurityAttributes,
            int dwCreationDisposition,
            FileFlags dwFlagsAndAttributes,
            int hTemplateFile);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadFile(SafeFileHandle hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead,
            out uint lpNumberOfBytesRead, [In] ref NativeOverlapped lpOverlapped);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll")]
        internal static extern bool WriteFile(SafeFileHandle hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten, [In] ref NativeOverlapped lpOverlapped);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetOverlappedResult(SafeHandle hFile,
            [In] ref NativeOverlapped lpOverlapped, out uint lpNumberOfBytesTransferred, [MarshalAs(UnmanagedType.Bool)]bool bWait);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern bool CancelIoEx(SafeFileHandle hFile, ref NativeOverlapped lpOverlapped);

        #endregion

        #region Device enumuration

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("setupapi.dll", EntryPoint = "SetupDiGetDeviceRegistryProperty")]
        internal static extern bool SetupDiGetDeviceRegistryProperty(
            SafeDevInfoHandle deviceInfoSet,
            [MarshalAs(UnmanagedType.LPStruct), In] SP_DEVINFO_DATA deviceInfoData,
            int propertyVal,
            ref int propertyRegDataType,
            byte[] propertyBuffer,
            int propertyBufferSize,
            ref int requiredSize);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("setupapi.dll", EntryPoint = "SetupDiGetDevicePropertyW", SetLastError = true)]
        internal static extern bool SetupDiGetDeviceProperty(
            SafeDevInfoHandle deviceInfo,
            [MarshalAs(UnmanagedType.LPStruct), In] SP_DEVINFO_DATA deviceInfoData,
            [MarshalAs(UnmanagedType.LPStruct), In] DEVPROPKEY propkey,
            ref ulong propertyDataType,
            byte[] propertyBuffer,
            int propertyBufferSize,
            ref int requiredSize,
            uint flags);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("setupapi.dll")]
        internal static extern bool SetupDiEnumDeviceInfo(SafeDevInfoHandle deviceInfoSet, int memberIndex, [MarshalAs(UnmanagedType.LPStruct), In] SP_DEVINFO_DATA deviceInfoData);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("setupapi.dll")]
        internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("setupapi.dll")]
        internal static extern bool SetupDiEnumDeviceInterfaces(
            SafeDevInfoHandle deviceInfoSet,
            [MarshalAs(UnmanagedType.LPStruct), In] SP_DEVINFO_DATA deviceInfoData,
            [MarshalAs(UnmanagedType.LPStruct), In] Guid interfaceClassGuid,
            int memberIndex,
            [MarshalAs(UnmanagedType.LPStruct), In] SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode)]
        internal static extern SafeDevInfoHandle SetupDiGetClassDevs(
            [MarshalAs(UnmanagedType.LPStruct), In] Guid classGuid,
            string? enumerator,
            IntPtr hwndParent,
            int flags);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, EntryPoint = "SetupDiGetDeviceInterfaceDetail")]
        internal static extern bool SetupDiGetDeviceInterfaceDetailBuffer(
            SafeDevInfoHandle deviceInfoSet,
            [MarshalAs(UnmanagedType.LPStruct), In] SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            IntPtr deviceInterfaceDetailData,
            int deviceInterfaceDetailDataSize,
            ref int requiredSize,
            IntPtr deviceInfoData);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(
            SafeDevInfoHandle deviceInfoSet,
            [MarshalAs(UnmanagedType.LPStruct), In] SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            [MarshalAs(UnmanagedType.LPStruct), In, Out] SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
            int deviceInterfaceDetailDataSize,
            ref int requiredSize,
            IntPtr deviceInfoData);

        #endregion

        #region HID property

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("hid.dll")]
        internal static extern bool HidD_GetAttributes(SafeFileHandle hidDeviceObject, HidAttributes attributes);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("hid.dll")]
        internal static extern void HidD_GetHidGuid(out Guid hidGuid);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("hid.dll")]
        internal static extern bool HidD_GetPreparsedData(SafeFileHandle hidDeviceObject, ref IntPtr preparsedData);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("hid.dll")]
        internal static extern bool HidD_FreePreparsedData(IntPtr preparsedData);

        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [DllImport("hid.dll")]
        internal static extern HIDP_STATUS HidP_GetCaps(SafePreParsedDataHandle preparsedData, [MarshalAs(UnmanagedType.LPStruct), In, Out] HidCapabilities capabilities);

        #endregion

        #region Error codes

        // HID API ERROR CODES
        // http://download.ros.org/downloads/spacenav/spacenav_svn/spacenav_win32/HIDLibrary/HidDeclarations.cs
        internal enum HIDP_STATUS : int
        {
            SUCCESS = (0x0 << 28) | (0x11 << 16) | 0,
            NULL = (0x8 << 28) | (0x11 << 16) | 1,
            INVALID_PREPARSED_DATA = (0xC << 28) | (0x11 << 16) | 1,
            INVALID_REPORT_TYPE = (0xC << 28) | (0x11 << 16) | 2,
            INVALID_REPORT_LENGTH = (0xC << 28) | (0x11 << 16) | 3,
            USAGE_NOT_FOUND = (0xC << 28) | (0x11 << 16) | 4,
            VALUE_OUT_OF_RANGE = (0xC << 28) | (0x11 << 16) | 5,
            BAD_LOG_PHY_VALUES = (0xC << 28) | (0x11 << 16) | 6,
            BUFFER_TOO_SMALL = (0xC << 28) | (0x11 << 16) | 7,
            INTERNAL_ERROR = (0xC << 28) | (0x11 << 16) | 8,
            I8042_TRANS_UNKNOWN = (0xC << 28) | (0x11 << 16) | 9,
            INCOMPATIBLE_REPORT_ID = (0xC << 28) | (0x11 << 16) | 0xA,
            NOT_VALUE_ARRAY = (0xC << 28) | (0x11 << 16) | 0xB,
            IS_VALUE_ARRAY = (0xC << 28) | (0x11 << 16) | 0xC,
            DATA_INDEX_NOT_FOUND = (0xC << 28) | (0x11 << 16) | 0xD,
            DATA_INDEX_OUT_OF_RANGE = (0xC << 28) | (0x11 << 16) | 0xE,
            BUTTON_NOT_PRESSED = (0xC << 28) | (0x11 << 16) | 0xF,
            REPORT_DOES_NOT_EXIST = (0xC << 28) | (0x11 << 16) | 0x10,
            NOT_IMPLEMENTED = (0xC << 28) | (0x11 << 16) | 0x20
        }

        #endregion
    }
}
