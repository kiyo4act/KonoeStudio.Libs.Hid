using System;
using System.Collections.Generic;
using System.Linq;

namespace KonoeStudio.Libs.Hid
{
    public class HidReport : IHidReport
    {
        public byte ReportId { get; }
        public IReadOnlyList<byte> Data => _data.ToList().AsReadOnly();
        private readonly byte[] _data;

        public HidReport(byte reportId, byte[] data)
        {
            ReportId = reportId;
            _data = data ?? Array.Empty<byte>();
        }

        public HidReport(params byte[] wholeData): this(wholeData, wholeData.Length)
        {
        }
        public HidReport(byte[] data, int reportAndDataSize)
        {
            if (data == null)
            {
                throw new ArgumentNullException($"{nameof(data)} is null");
            }
            if (reportAndDataSize <= 0)
            {
                throw new ArgumentException($"{nameof(reportAndDataSize)} is required more than {0}");
            }
            else if (data.Length < reportAndDataSize)
            {
                throw new ArgumentException($"{nameof(reportAndDataSize)} is required up to {nameof(data)} length: {data.Length}");
            }
            else if (reportAndDataSize == 1)
            {
                ReportId = data[0];
                _data = Array.Empty<byte>();
            }
            else
            {
                _data = new byte[reportAndDataSize - 1];
                Array.Copy(data, 1, _data, 0, reportAndDataSize - 1);
            }
        }
    }
}
