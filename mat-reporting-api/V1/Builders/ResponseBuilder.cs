using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MaTReportingAPI.V1.Builders
{
    public static class ResponseBuilder
    {
        public static JsonResult Ok(object responseContent)
        {
            var jsonResponse = new JsonResult(responseContent)
            {
                StatusCode = 200,
                ContentType = "application/json"
            };

            return jsonResponse;
        }

        public static JsonResult Error(int statusCode, string message)
        {
            return new JsonResult(message)
            {
                StatusCode = statusCode,
                ContentType = "application/json"

            };
        }

        public static JsonResult ErrorFromList(int statusCode, IEnumerable<string> messages)
        {
            return new JsonResult(messages)
            {
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }
    }
}
