using MaTReportingAPI.V1.CustomExceptions;
using Newtonsoft.Json;
using System.Net.Http;

namespace MaTReportingAPI.V1.Gateways
{
    public class CRMGateway : ICRMGateway
    {
        private readonly ICRMTokenGateway _CRMTokenGateway;
        private readonly HttpClient _httpClient;

        public CRMGateway(ICRMTokenGateway crmTokenGateway, HttpClient httpClient)
        {
            _CRMTokenGateway = crmTokenGateway;
            _httpClient = httpClient;
        }

        public dynamic GetEntitiesByFetchXMLQuery(string entityType, string query)
        {
            //same HttpClient from http client factory will be used so check if the Authorization header has been set already
            //will throw custom exception on failure, let it bubble up to controller
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                string accessToken = _CRMTokenGateway.GetCRMAccessToken();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            }

            try
            {
                string queryURL = $"/api/data/v8.2/{entityType}?fetchXml={query}";

                HttpResponseMessage httpResponseMessage = _httpClient.GetAsync(queryURL).Result;

                string result = httpResponseMessage.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<dynamic>(result).value;
            }
            catch
            {
                throw new CRMException("Unable to fetch data from CRM");
            }
        }
    }
}
