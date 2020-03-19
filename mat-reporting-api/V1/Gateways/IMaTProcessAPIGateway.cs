using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MaTReportingAPI.V1.Gateways
{
    public interface IMaTProcessAPIGateway
    {
        JObject GetHomeCheckAnswersByInteractionIDs(List<string> interactionIDs);
    }
}
