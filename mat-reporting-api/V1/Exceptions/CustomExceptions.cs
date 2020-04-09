using System;

namespace MaTReportingAPI.V1.Exceptions
{
    //base class to make it easier to handle custom exceptions at controller level
    public class MaTReportingApiBaseException : Exception
    {
        //constructor required for testing
        public MaTReportingApiBaseException() { }
        public MaTReportingApiBaseException(string message) : base(message) { }
    }

    public class MaTProcessApiException : MaTReportingApiBaseException
    {
        public MaTProcessApiException() { }
        public MaTProcessApiException(string message) : base(message) {}
    }

    public class CRMTokenException : MaTReportingApiBaseException
    {
        public CRMTokenException() { }
        public CRMTokenException(string message) : base(message) {}
    }

    public class CRMException : MaTReportingApiBaseException
    {
        public CRMException() { }
        public CRMException(string message) : base(message) { }
    }

    public class MaTProcessDbException : MaTReportingApiBaseException
    {
        public MaTProcessDbException() { }
        public MaTProcessDbException(string message) : base(message) { }
    }
}
