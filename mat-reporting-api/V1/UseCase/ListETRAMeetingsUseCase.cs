using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Gateways;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MaTReportingAPI.UseCase.V1
{
    public class ListETRAMeetingsUseCase : IListETRAMeetings
    {
        private readonly IETRAMeetingsGateway _eTRAMeetingsGateway;

        public ListETRAMeetingsUseCase(IETRAMeetingsGateway eTRAMeetingsGateway)
        {
            _eTRAMeetingsGateway = eTRAMeetingsGateway;
        }

        public ListETRAMeetingsResponse Execute(ListETRAMeetingsRequest request)
        {
            var result = _eTRAMeetingsGateway.GetETRAMeetingsByDateRange(request.FromDate, request.ToDate);
                       
            return new ListETRAMeetingsResponse(result, request, DateTime.Now, result.Count, result.Sum(x => x.NoOfActions));
        }
    }
}
