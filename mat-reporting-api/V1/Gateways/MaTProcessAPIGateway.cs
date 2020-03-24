using MaTReportingAPI.V1.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;

namespace MaTReportingAPI.V1.Gateways
{
    public class MaTProcessAPIGateway : IMaTProcessAPIGateway
    {
        private readonly HttpClient _httpClient;

        public MaTProcessAPIGateway(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public JObject GetHomeCheckAnswersByInteractionIDs(List<string> interactionIDs)
        {
            try
            {
                var homeCheckResponse = _httpClient.PostAsJsonAsync("", interactionIDs).Result;

                string homeCheckAnswers = homeCheckResponse.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<JObject>(homeCheckAnswers);
            }
            catch
            {
                throw new MaTProcessApiException("Unable to get home check details from MaT Process API");
            }
        }
    }
}
