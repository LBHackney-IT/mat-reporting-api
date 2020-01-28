using System;

namespace MaTReportingAPI.UseCase.V1
{
    public class TestOpsErrorException : Exception
    {
        public TestOpsErrorException() : base("This is a test exception to test our integrations"){}

        public TestOpsErrorException(string message) : base(message)
        {
        }

        public TestOpsErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
