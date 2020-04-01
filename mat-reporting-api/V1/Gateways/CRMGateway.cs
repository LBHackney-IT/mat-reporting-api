using MaTReportingAPI.V1.Exceptions;
using Microsoft.AspNetCore.Http;
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
            //make sure Authorization header has been set
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _CRMTokenGateway.GetCRMAccessToken());
            }
            
            try
            {
                string queryURL = $"/api/data/v8.2/{entityType}?fetchXml={query}";
               
                HttpResponseMessage httpResponseMessage = _httpClient.GetAsync(queryURL).Result;

                //Deal with Unauthorized response, typically caused by expired token. Try again with updated token
                //faster and safer than manually checking whether current token has expired
                if((int)httpResponseMessage.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _CRMTokenGateway.GetCRMAccessToken());

                    httpResponseMessage = _httpClient.GetAsync(queryURL).Result;
                }

                if((int)httpResponseMessage.StatusCode == StatusCodes.Status200OK)
                {
                    string result = httpResponseMessage.Content.ReadAsStringAsync().Result;

                    return JsonConvert.DeserializeObject<dynamic>(result).value;
                }
                //could't recover from unauthorized error, or something else went wrong
                else
                {
                    throw new CRMException("Unable to fetch data from CRM");
                }
            }
            catch
            {
                throw new CRMException("Unable to fetch data from CRM");
            }
        }
    }
}
