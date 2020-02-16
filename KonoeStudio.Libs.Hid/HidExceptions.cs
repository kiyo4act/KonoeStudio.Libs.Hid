using System;
using System.Runtime.Serialization;

namespace KonoeStudio.Libs.Hid
{
    [Serializable]
    public class GetHidAttributesException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public GetHidAttributesException()
        {
        }

        public GetHidAttributesException(string message) : base(message)
        {
        }

        public GetHidAttributesException(string message, Exception inner) : base(message, inner)
        {
        }

        protected GetHidAttributesException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class GetHidCapabilitiesException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public GetHidCapabilitiesException()
        {
        }

        public GetHidCapabilitiesException(string message) : base(message)
        {
        }

        public GetHidCapabilitiesException(string message, Exception inner) : base(message, inner)
        {
        }

        protected GetHidCapabilitiesException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class DeviceIsNotOpenedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public DeviceIsNotOpenedException() { }
        public DeviceIsNotOpenedException(string message) : base(message) { }
        public DeviceIsNotOpenedException(string message, Exception inner) : base(message, inner) { }

        protected DeviceIsNotOpenedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class HasNotCapabilityException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public HasNotCapabilityException() { }
        public HasNotCapabilityException(string message) : base(message) { }
        public HasNotCapabilityException(string message, Exception inner) : base(message, inner) { }

        protected HasNotCapabilityException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class DeviceCouldNotOpenedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public DeviceCouldNotOpenedException() { }
        public DeviceCouldNotOpenedException(string message) : base(message) { }
        public DeviceCouldNotOpenedException(string message, Exception inner) : base(message, inner) { }

        protected DeviceCouldNotOpenedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class ReportException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ReportException() { }
        public ReportException(string message) : base(message) { }
        public ReportException(string message, Exception inner) : base(message, inner) { }

        protected ReportException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
