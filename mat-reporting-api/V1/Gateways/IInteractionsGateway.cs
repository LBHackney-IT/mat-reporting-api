using MaTReportingAPI.V1.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MaTReportingAPI.V1.Gateways
{
    public interface IInteractionsGateway
    {
        List<Interaction> GetInteractionsByDateRange(string fromDate, string toDate);

        List<ParentInteraction> GetInteractionsAndChildInteractionsByDateRange(string fromDate, string toDate);
    }
}
