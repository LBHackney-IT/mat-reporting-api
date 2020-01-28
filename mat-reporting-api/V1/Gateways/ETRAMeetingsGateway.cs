using MaTReportingAPI.V1.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MaTReportingAPI.V1.Gateways
{
    public class ETRAMeetingsGateway : IETRAMeetingsGateway
    {
        private static readonly string GetCRM365AccessTokenURL = Environment.GetEnvironmentVariable("GetCRM365AccessTokenURL");
        private static readonly string MaTProcessAPIURL = Environment.GetEnvironmentVariable("MaTProcessAPIURL");
        private static readonly string CRMAPIBaseURL = Environment.GetEnvironmentVariable("CRMAPIBaseURL");

        public List<ETRAMeeting> GetETRAMeetingsByDateRange(string fromDate, string toDate)
        {
            HttpClient _client = new HttpClient();

            var tokenResponseMessage = _client.GetAsync(GetCRM365AccessTokenURL).Result;

            string r = tokenResponseMessage.Content.ReadAsStringAsync().Result;
            var tokenJsonResponse = JsonConvert.DeserializeObject<JObject>(r);
            var accessToken = tokenJsonResponse["result"].ToString();
     
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            _client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            _client.DefaultRequestHeaders.Add("OData-Version", "4.0");

            string parentMeetingsQuery =
            $@"
            <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
              <entity name='hackney_tenancymanagementinteractions'>
                <attribute name='hackney_tenancymanagementinteractionsid' />
                <attribute name='hackney_name' />
                <attribute name='createdon' />
                <order attribute='hackney_name' descending='false' />
                <filter type='and'>
                    <condition attribute='hackney_natureofenquiry' operator='eq' value='28' />
                    <condition attribute='createdon' value='{fromDate}' operator='on-or-after'/>
                    <condition attribute='createdon' value='{toDate}' operator='on-or-before' />
                </filter>
              </entity>
            </fetch>
            ";

            string parentMeetingsFetchUrl = $"{CRMAPIBaseURL}/hackney_tenancymanagementinteractionses?fetchXml={parentMeetingsQuery}";

            HttpResponseMessage response = _client.GetAsync(parentMeetingsFetchUrl).Result;

            string meetingsResult = response.Content.ReadAsStringAsync().Result;

            var parentMeetings = JsonConvert.DeserializeObject<dynamic>(meetingsResult).value;
          
            List<ETRAMeeting> meetings = new List<ETRAMeeting>();
           
            foreach (var m in parentMeetings)
            {
                string meetingID = m.hackney_tenancymanagementinteractionsid; 
                               
                string queryForChildMeetings =
                $@"
                    <fetch  version='1.0' distinct='false' mapping='logical' output-format='xml-platform'>
                    <entity name='hackney_tenancymanagementinteractions'>
                    <attribute name='hackney_tenancymanagementinteractionsid'/>
                    <attribute name='hackney_name'/>
                    <attribute name='createdon'/>
                    <attribute name='hackney_parent_interactionid' />
                    <order descending='false' attribute='hackney_name'/>
                    <filter type='and'>
                    <condition attribute='hackney_parent_interactionid' operator='eq' value='{meetingID}'/>
                    </filter>
                    </entity>
                    </fetch> 
                ";

                ETRAMeeting meeting = new ETRAMeeting() {
                    Name = m.hackney_name
                };
                
                string childMeetingsFetchUrl = $"{CRMAPIBaseURL}/hackney_tenancymanagementinteractionses?fetchXml={queryForChildMeetings}";

                HttpResponseMessage childMeetingsResponse = _client.GetAsync(childMeetingsFetchUrl).Result;

                var childMeetingsResult = childMeetingsResponse.Content.ReadAsStringAsync().Result;
                
                meeting.NoOfActions = JsonConvert.DeserializeObject<dynamic>(childMeetingsResult)?.value?.Count;

                meetings.Add(meeting);
            }

            return meetings;
        }
       
    }
}
