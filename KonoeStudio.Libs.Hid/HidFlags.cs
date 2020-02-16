using System;

namespace KonoeStudio.Libs.Hid
{
    // Reference: CreateFileA function
    // https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea

#pragma warning disable CA1028 // Need to uint because of native flags
    [Flags]
    public enum ShareModes : uint
    {
        FileShareNone = 0x00000000,
        FileShareRead = 0x00000001,
        FileShareWrite = 0x00000002,
        FileShareDelete = 0x00000004
    }

#pragma warning disable CA1028 // Need to uint because of native flags
    [Flags]
    public enum DesiredAccesses : uint
    {
        AccessNone = 0x00000000,
        GenericWrite = 0x40000000,
        GenericRead = 0x80000000
    }

#pragma warning disable CA1028 // Need to uint because of native flags
    [Flags]
    public enum FileFlags : uint
    {
        FileFlagNone = 0x00000000,
        FileFlagOverlapped = 0x40000000
    }
}
