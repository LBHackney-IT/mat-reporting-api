using MaTReportingAPI.V1.CustomExceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace MaTReportingAPI.V1.Gateways
{
    public class CRMTokenGateway : ICRMTokenGateway
    {
        private readonly HttpClient _httpClient;
        
        public CRMTokenGateway(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //TODO: throw token exception
        public string GetCRMAccessToken()
        {
            try
            {
                var tokenResponseMessage = _httpClient.GetAsync("").Result;

                string r = tokenResponseMessage.Content.ReadAsStringAsync().Result;
                var tokenJsonResponse = JsonConvert.DeserializeObject<JObject>(r);
                var accessToken = tokenJsonResponse["result"].ToString();

                return accessToken;
            }
            catch
            {
                throw new CRMTokenException("Unable to retrieve CRM access token");
            }
        }
    }
}
