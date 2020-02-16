using System;
using System.Linq;
using KonoeStudio.Libs.Hid;
using NUnit.Framework;

namespace KonoeStudio.Tests.Hid
{
    [TestFixture()]
    public class HidReportTests
    {
        [TestCase(0x00, new byte[]{0x00, 0x01, 0x02, 0x03, 0x04})]
        [TestCase(0x01, new byte[] { 0x01})]
        [TestCase(0x02, new byte[] { })]
        public void HidReportTest(byte reportId, byte[] data)
        {
            var report = new HidReport(reportId, data);
            report.IsNotNull();
            report.ReportId.Is(reportId);
            report.Data.IsNotNull();
            report.Data.SequenceEqual(data);
        }

        [TestCase(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 })]
        [TestCase(new byte[] { 0x01 })]
        public void HidReportTest1(byte[] data)
        {
            var report = new HidReport(data);
            report.IsNotNull();
            report.ReportId.Is(data[0]);
            report.Data.IsNotNull();
            report.Data.SequenceEqual(data.ToList().GetRange(1, data.Length - 1));
        }

        [TestCase( new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 }, 5)]
        [TestCase( new byte[] { 0x01 }, 1)]
        public void HidReportTest2(byte[] data, int size)
        {
            var report = new HidReport(data, size);
            report.IsNotNull();
            report.ReportId.Is(data[0]);
            report.Data.IsNotNull();
            report.Data.SequenceEqual(data.ToList().GetRange(1, size - 1));
        }

        [Test]
        public void HidReportExceptionTest()
        {
            Assert.Catch<ArgumentNullException>(() => new HidReport(null, 1));
            Assert.Catch<ArgumentException>(() => new HidReport(new byte[]{0x00}, 0));
            Assert.Catch<ArgumentException>(() => new HidReport(new byte[] { 0x00 }, 2));
        }
    }
}