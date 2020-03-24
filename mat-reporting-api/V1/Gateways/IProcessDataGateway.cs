using MaTReportingAPI.V1.Domain;
using System.Collections.Generic;

namespace MaTReportingAPI.V1.Gateways
{
    public interface  IProcessDataGateway
    {
        List<MatProcessData> GetProcessDataByIDs(string[] refs);
    }
}
