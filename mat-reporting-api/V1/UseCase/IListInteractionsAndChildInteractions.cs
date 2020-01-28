using System.Threading.Tasks;

namespace MaTReportingAPI.V1.Boundary
{
    public interface IListInteractionsAndChildInteractions
    {
        ListInteractionsAndChildInteractionsResponse Execute(ListInteractionsAndChildInteractionsRequest request);
    }
}
