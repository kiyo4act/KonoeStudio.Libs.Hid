using System.Collections.Generic;

namespace KonoeStudio.Libs.Hid
{
    public interface IHidReport
    {
        byte ReportId { get; }
        IReadOnlyList<byte> Data { get; }
    }
}
