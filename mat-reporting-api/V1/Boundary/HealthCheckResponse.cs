using System;

namespace MaTReportingAPI.V1.Boundary
{
    public class HealthCheckResponse
    {
        public HealthCheckResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public readonly bool Success;
        public readonly string Message;
    }
}
