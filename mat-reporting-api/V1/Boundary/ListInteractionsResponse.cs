using MaTReportingAPI.V1.Domain;
using System;
using System.Collections.Generic;

namespace MaTReportingAPI.V1.Boundary
{
    public class ListInteractionsResponse
    {
        public readonly ListInteractionsRequest Request;
        public readonly DateTime GeneratedAt;
        public readonly List<Interaction> Interactions;

        public ListInteractionsResponse(ListInteractionsRequest request, DateTime generatedAt, List<Interaction> interactions)
        {
            Request = request;
            GeneratedAt = generatedAt;
            Interactions = interactions;
        }
    }
}
