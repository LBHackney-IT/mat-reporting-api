using MaTReportingAPI.V1.Domain;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MaTReportingAPI.V1.Gateways
{
    public class ETRAMeetingsGateway : IETRAMeetingsGateway
    {
        private readonly ICRMGateway _CRMGateway;
        private static string _tenancyManagementInteractionEntityName = "hackney_tenancymanagementinteractionses";

        public ETRAMeetingsGateway(ICRMGateway CRMGateway)
        {
            _CRMGateway = CRMGateway;
        }

        public List<ETRAMeeting> GetETRAMeetingsByDateRange(string fromDate, string toDate)
        {
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

            dynamic parentMeetings = _CRMGateway.GetEntitiesByFetchXMLQuery(_tenancyManagementInteractionEntityName, parentMeetingsQuery);

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

                //TODO: check if we need to add more properties
                ETRAMeeting meeting = new ETRAMeeting() {
                    Name = m.hackney_name
                };

                var actions = _CRMGateway.GetEntitiesByFetchXMLQuery(_tenancyManagementInteractionEntityName, queryForChildMeetings);

                meeting.NoOfActions = ((JArray)actions).Count;

                meetings.Add(meeting);
            }

            return meetings;
        }
    }
}
