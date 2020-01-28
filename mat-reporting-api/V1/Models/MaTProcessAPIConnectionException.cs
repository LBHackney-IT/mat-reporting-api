using System;

namespace MaTReportingAPI.V1.Models
{
    public class MaTProcessAPIConnectionException : Exception
    {
        public MaTProcessAPIConnectionException()
        {
        }

        public MaTProcessAPIConnectionException(string message)
            : base(message)
        {
        }
        public MaTProcessAPIConnectionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
