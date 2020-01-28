using MaTReportingAPI.V1.Boundary;
using MaTReportingAPI.V1.Gateways;
using System;
using System.Threading.Tasks;

namespace MaTReportingAPI.UseCase.V1
{
    public class ListInteractionsUseCase : IListInteractions
    {
        private readonly IInteractionsGateway _interactionsGateway;

        public ListInteractionsUseCase(IInteractionsGateway interactionsGateway)
        {
            _interactionsGateway = interactionsGateway;
        }

        public ListInteractionsResponse Execute(ListInteractionsRequest request)
        {
            var result = _interactionsGateway.GetInteractionsByDateRange(request.FromDate, request.ToDate);

            return new ListInteractionsResponse(request, DateTime.Now, result);
        }
    }
}
