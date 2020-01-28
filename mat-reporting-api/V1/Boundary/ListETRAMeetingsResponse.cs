using MaTReportingAPI.V1.Domain;
using System;
using System.Collections.Generic;

namespace MaTReportingAPI.V1.Boundary
{
    public class ListETRAMeetingsResponse
    {
        public readonly ListETRAMeetingsRequest Request;
        public readonly DateTime GeneratedAt;
        public readonly int TotalNoOfMeetings;
        public readonly int TotalNoOfActions;
        public readonly List<ETRAMeeting> ETRAMeetings;
     
        public ListETRAMeetingsResponse(
            List<ETRAMeeting> eTRAMeetings,
            ListETRAMeetingsRequest request,
            DateTime generatedAt,
            int totalNoOfMeetings,
            int totalNoOfActions)
        {
            Request = request;
            GeneratedAt = generatedAt;
            ETRAMeetings = eTRAMeetings;
            TotalNoOfMeetings = totalNoOfMeetings;
            TotalNoOfActions = totalNoOfActions;
        }
    }
}
