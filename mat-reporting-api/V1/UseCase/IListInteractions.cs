using System.Threading.Tasks;

namespace MaTReportingAPI.V1.Boundary
{
    public interface IListInteractions
    {
        ListInteractionsResponse Execute(ListInteractionsRequest request);
    }
}
