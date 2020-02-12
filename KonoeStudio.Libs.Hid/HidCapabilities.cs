using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace KonoeStudio.Libs.Hid
{
    [StructLayout(LayoutKind.Sequential)]
    public class HidCapabilities
    {
        private readonly short _usage;

        private readonly short _usagePage;

        private readonly short _inputReportByteLength;

        private readonly short _outputReportByteLength;

        private readonly short _featureReportByteLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        private readonly short[] _reserved;

        private readonly short _numberLinkCollectionNodes;

        private readonly short _numberInputButtonCaps;

        private readonly short _numberInputValueCaps;

        private readonly short _numberInputDataIndices;

        private readonly short _numberOutputButtonCaps;

        private readonly short _numberOutputValueCaps;

        private readonly short _numberOutputDataIndices;

        private readonly short _numberFeatureButtonCaps;

        private readonly short _numberFeatureValueCaps;

        private readonly short _numberFeatureDataIndices;
        public short Usage => _usage;

        public short UsagePage => _usagePage;

        public short InputReportByteLength => _inputReportByteLength;

        public short OutputReportByteLength => _outputReportByteLength;

        public short FeatureReportByteLength => _featureReportByteLength;

        public IReadOnlyList<short> Reserved => _reserved.ToList().AsReadOnly();

        public short NumberLinkCollectionNodes => _numberLinkCollectionNodes;

        public short NumberInputButtonCaps => _numberInputButtonCaps;

        public short NumberInputValueCaps => _numberInputValueCaps;

        public short NumberInputDataIndices => _numberInputDataIndices;

        public short NumberOutputButtonCaps => _numberOutputButtonCaps;

        public short NumberOutputValueCaps => _numberOutputValueCaps;

        public short NumberOutputDataIndices => _numberOutputDataIndices;

        public short NumberFeatureButtonCaps => _numberFeatureButtonCaps;

        public short NumberFeatureValueCaps => _numberFeatureValueCaps;

        public short NumberFeatureDataIndices => _numberFeatureDataIndices;

        public HidCapabilities()
        {
            _reserved = new short[17];
        }

        public HidCapabilities(short usage, short usagePage, short inputReportByteLength, short outputReportByteLength, short featureReportByteLength, short[] reserved, short numberLinkCollectionNodes, short numberInputButtonCaps, short numberInputValueCaps, short numberInputDataIndices, short numberOutputButtonCaps, short numberOutputValueCaps, short numberOutputDataIndices, short numberFeatureButtonCaps, short numberFeatureValueCaps, short numberFeatureDataIndices)
        {
            _usage = usage;
            _usagePage = usagePage;
            _inputReportByteLength = inputReportByteLength;
            _outputReportByteLength = outputReportByteLength;
            _featureReportByteLength = featureReportByteLength;
            _reserved = reserved;
            _numberLinkCollectionNodes = numberLinkCollectionNodes;
            _numberInputButtonCaps = numberInputButtonCaps;
            _numberInputValueCaps = numberInputValueCaps;
            _numberInputDataIndices = numberInputDataIndices;
            _numberOutputButtonCaps = numberOutputButtonCaps;
            _numberOutputValueCaps = numberOutputValueCaps;
            _numberOutputDataIndices = numberOutputDataIndices;
            _numberFeatureButtonCaps = numberFeatureButtonCaps;
            _numberFeatureValueCaps = numberFeatureValueCaps;
            _numberFeatureDataIndices = numberFeatureDataIndices;
        }
    }
}
