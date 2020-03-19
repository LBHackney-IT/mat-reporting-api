namespace MaTReportingAPI.V1.Gateways
{
    public interface ICRMGateway
    {
        dynamic GetEntitiesByFetchXMLQuery(string entityType, string query);
    }
}
