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

        //TODO: error handling, pass api version as param
        public dynamic GetEntitiesByFetchXMLQuery(string entityType, string query)
        {
            //same HttpClient from http client factory will be used so check if the Authorization header has been set already
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                string accessToken = _CRMTokenGateway.GetCRMAccessToken();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            }

            string queryURL = $"/api/data/v8.2/{entityType}?fetchXml={query}";

            HttpResponseMessage httpResponseMessage = _httpClient.GetAsync(queryURL).Result;

            //TODO: add entity error handling
            string result = httpResponseMessage.Content.ReadAsStringAsync().Result;

            //TODO: null result
            return JsonConvert.DeserializeObject<dynamic>(result).value;
        }
    }
}
