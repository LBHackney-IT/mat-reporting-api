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

        //TODO: make async
        public JObject GetHomeCheckAnswersByInteractionIDs(List<string> interactionIDs)
        {
            var homeCheckResponse = _httpClient.PostAsJsonAsync("", interactionIDs).Result;

            string homeCheckAnswers = homeCheckResponse.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<JObject>(homeCheckAnswers);
        }
    }
}
