using MaTReportingAPI.V1.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MaTReportingAPI.V1.Gateways
{
    public interface IETRAMeetingsGateway
    {
        List<ETRAMeeting> GetETRAMeetingsByDateRange(string fromDate, string toDate);
    }
}
