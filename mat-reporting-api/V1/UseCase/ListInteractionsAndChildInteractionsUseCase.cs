using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Gateways;
using System;
using System.Threading.Tasks;

namespace MaTReportingAPI.UseCase.V1
{
    public class ListInteractionsAndChildInteractionsUseCase : IListInteractionsAndChildInteractions
    {
        private readonly IInteractionsGateway _interactionsGateway;

        public ListInteractionsAndChildInteractionsUseCase(IInteractionsGateway interactionsGateway)
        {
            _interactionsGateway = interactionsGateway;
        }

        public ListInteractionsAndChildInteractionsResponse Execute(ListInteractionsAndChildInteractionsRequest request)
        {
            var result = _interactionsGateway.GetInteractionsAndChildInteractionsByDateRange(request.FromDate, request.ToDate);

            return new ListInteractionsAndChildInteractionsResponse(request, DateTime.Now, result);
        }
    }
}
