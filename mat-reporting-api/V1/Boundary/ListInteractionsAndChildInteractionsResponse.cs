using MaTReportingAPI.V1.Domain;
using System;
using System.Collections.Generic;

namespace MaTReportingAPI.V1.Boundary
{
    public class ListInteractionsAndChildInteractionsResponse
    {
        public ListInteractionsAndChildInteractionsRequest Request { get; set; }
        public DateTime GeneratedAt { get; set; }
        public List<Interaction> Interactions { get; set; }

        public ListInteractionsAndChildInteractionsResponse(
            ListInteractionsAndChildInteractionsRequest request,
            DateTime generatedAt,
            List<Interaction> interactions)
        {
            Request = request;
            GeneratedAt = generatedAt;
            Interactions = interactions;
        }
    }
}
