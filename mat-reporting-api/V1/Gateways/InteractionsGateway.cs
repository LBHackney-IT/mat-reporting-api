using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MaTReportingAPI.V1.Gateways
{
    public class InteractionsGateway : IInteractionsGateway
    {
        private static readonly string GetCRM365AccessTokenURL = Environment.GetEnvironmentVariable("GetCRM365AccessTokenURL");
        private static readonly string MaTProcessAPIURL = Environment.GetEnvironmentVariable("MaTProcessAPIURL");
        private static readonly string CRMAPIBaseURL = Environment.GetEnvironmentVariable("CRMAPIBaseURL");

        public List<Interaction> GetInteractionsByDateRange(string fromDate, string toDate)
        {
            HttpClient _client = new HttpClient();

            HttpResponseMessage tokenResponse = _client.GetAsync(GetCRM365AccessTokenURL).Result;
            string r = tokenResponse.Content.ReadAsStringAsync().Result;
            var tokenJsonResponse = JsonConvert.DeserializeObject<JObject>(r);
            var accessToken = tokenJsonResponse["result"].ToString();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            _client.DefaultRequestHeaders.Add("OData-Version", "4.0");
            _client.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=\"OData.Community.Display.V1.FormattedValue\"");

            string interactionsQuery =
            $@"
            <fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>
                <entity name='hackney_tenancymanagementinteractions'>
                <attribute name='hackney_tenancymanagementinteractionsid'/>
                <attribute name='hackney_name'/>
                <attribute name='createdon'/>
                <attribute name='hackney_estateofficer_updatedbyid'/>
                <attribute name='hackney_transferred'/>
                <attribute name='hackney_subjectid'/>
                <attribute name='hackney_reasonforstartingprocess'/>
                <attribute name='hackney_processtype'/>
                <attribute name='hackney_process_stage'/>
                <attribute name='hackney_areaname'/>
                <attribute name='hackney_natureofenquiry'/>
                <attribute name='hackney_managerpropertypatchid'/>
                <attribute name='hackney_incidentid'/>
                <attribute name='hackney_household_interactionid'/>
                <attribute name='hackney_handleby'/>
                <attribute name='hackney_estateofficerpatchid'/>
                <attribute name='hackney_enquirysubject'/>
                <attribute name='hackney_estateofficer_createdbyid'/>
                <attribute name='hackney_contactid'/>
                <order descending='false' attribute='hackney_name'/>
                <filter type='and'>
                    condition attribute='hackney_natureofenquiry' value='15' operator='eq'/>
                    <condition attribute='hackney_enquirysubject' value='100000156' operator='eq'/>
                    <condition attribute='createdon' value='{fromDate}' operator='on-or-after'/>
                    <condition attribute='createdon' value='{toDate}' operator='on-or-before'/>
                </filter>
                <link-entity name='contact' link-type='outer' visible='false' to='hackney_contactid' from='contactid'>
                    <attribute name='address1_postalcode'/>
                    <attribute name='address1_line3'/>
                    <attribute name='address1_line2'/>
                    <attribute name='address1_line1'/>
                </link-entity>
                </entity>
                </fetch>
            ";

            string url = $"{CRMAPIBaseURL}/hackney_tenancymanagementinteractionses?fetchXml={interactionsQuery}";

            HttpResponseMessage response = _client.GetAsync(url).Result;
            string interactionsResult = response.Content.ReadAsStringAsync().Result;
            var interactionsValue = JsonConvert.DeserializeObject<JObject>(interactionsResult);
            List<JToken> interactionsResponse = interactionsValue["value"].ToList();
            List<Interaction> interactions = new List<Interaction>();

            foreach (JToken m in interactionsResponse)
            {
                var id = m["hackney_tenancymanagementinteractionsid"]; 

                Interaction interaction = new Interaction()
                {
                    Id = m["hackney_tenancymanagementinteractionsid"]?.ToString(),
                    Name = m["hackney_name"]?.ToString(),
                    CreatedOn = m["createdon"]?.ToString(),
                    Contact = m["_hackney_contactid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    NatureofEnquiry = m["hackney_natureofenquiry@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    CreatedByEstateOfficer = m["_hackney_estateofficer_createdbyid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    EnquirySubject = m["hackney_enquirysubject@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    EstateOfficerPatch = m["_hackney_estateofficerpatchid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    HandledBy = m["hackney_handleby@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    HouseholdInteraction = m["_hackney_household_interactionid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    Incident = m["_hackney_incidentid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    ManagerPatch = m["_hackney_managerpropertypatchid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    NHOAreaName = m["hackney_areaname@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    ProcessStage = m["hackney_process_stage@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    ProcessType = m["hackney_processtype@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    ReasonForStartingProcess = m["hackney_reasonforstartingprocess@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    Subject = m["_hackney_subjectid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    Transferred = m["hackney_transferred@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    UpdatedByEstateOfficer = m["_hackney_estateofficer_updatedbyid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    AddressStreet1 = m["contact1_x002e_address1_line1"]?.ToString(),
                    AddressStreet2 = m["contact1_x002e_address1_line2"]?.ToString(),
                    AddressStreet3 = m["contact1_x002e_address1_line3"]?.ToString(),
                    AddressZIPPostalCode = m["contact1_x002e_address1_postalcode"]?.ToString()
                };

                interactions.Add(interaction);
            }

            List<string> interactionIDs = interactions.Select(x => x.Id).ToList();

            HttpResponseMessage homeCheckResponse;

            try
            {
                homeCheckResponse = _client.PostAsJsonAsync(MaTProcessAPIURL, interactionIDs).Result;
            }
            catch
            {
                throw new MaTProcessAPIConnectionException("Unable to connect to MaT Process API");
            }

            string homeCheckAnswers = homeCheckResponse.Content.ReadAsStringAsync().Result;

            var homeChecks = JsonConvert.DeserializeObject<JObject>(homeCheckAnswers);

            foreach(var homeCheck in homeChecks)
            {
                Interaction interaction = interactions.FirstOrDefault(x => x.Id == homeCheck.Key);
                interaction.HomeCheck = homeCheck.Value?.ToString();
            }

            return interactions;
        }

        public List<ParentInteraction> GetInteractionsAndChildInteractionsByDateRange(string fromDate, string toDate)
        {
            HttpClient _client = new HttpClient();

            HttpResponseMessage tokenResponse = _client.GetAsync(GetCRM365AccessTokenURL).Result;
            string r = tokenResponse.Content.ReadAsStringAsync().Result;
            var tokenJsonResponse = JsonConvert.DeserializeObject<JObject>(r);
            var accessToken = tokenJsonResponse["result"].ToString();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            _client.DefaultRequestHeaders.Add("OData-Version", "4.0");
            _client.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=\"OData.Community.Display.V1.FormattedValue\"");

            //get all parent interactions for the selected period
            string parentInteractionsQuery =
            $@"
            <fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>
                <entity name='hackney_tenancymanagementinteractions'>
                <attribute name='hackney_tenancymanagementinteractionsid'/>
                <attribute name='hackney_name'/>
                <attribute name='createdon'/>
                <attribute name='hackney_estateofficer_updatedbyid'/>
                <attribute name='hackney_transferred'/>
                <attribute name='hackney_subjectid'/>
                <attribute name='hackney_reasonforstartingprocess'/>
                <attribute name='hackney_processtype'/>
                <attribute name='hackney_process_stage'/>
                <attribute name='hackney_areaname'/>
                <attribute name='hackney_natureofenquiry'/>
                <attribute name='hackney_managerpropertypatchid'/>
                <attribute name='hackney_incidentid'/>
                <attribute name='hackney_household_interactionid'/>
                <attribute name='hackney_handleby'/>
                <attribute name='hackney_estateofficerpatchid'/>
                <attribute name='hackney_enquirysubject'/>
                <attribute name='hackney_estateofficer_createdbyid'/>
                <attribute name='hackney_contactid'/>
                <order descending='false' attribute='hackney_name'/>
                <filter type='and'>
                    <condition attribute='hackney_processtype' value='1' operator='eq'/>
                    <condition attribute='hackney_natureofenquiry' value='15' operator='eq'/>
                    <condition attribute='hackney_enquirysubject' value='100000156' operator='eq'/>
                    <condition attribute='createdon' value='{fromDate}' operator='on-or-after'/>
                    <condition attribute='createdon' value='{toDate}' operator='on-or-before'/>
                </filter>
                <link-entity name='contact' link-type='outer' visible='false' to='hackney_contactid' from='contactid'>
                    <attribute name='address1_postalcode'/>
                    <attribute name='address1_line3'/>
                    <attribute name='address1_line2'/>
                    <attribute name='address1_line1'/>
                </link-entity>
                </entity>
                </fetch>
            ";

            string url = $"{CRMAPIBaseURL}/hackney_tenancymanagementinteractionses?fetchXml={parentInteractionsQuery}";

            HttpResponseMessage response = _client.GetAsync(url).Result;

            string interactionsResult = response.Content.ReadAsStringAsync().Result;
            var interactionsValue = JsonConvert.DeserializeObject<JObject>(interactionsResult);
            List<JToken> interactionsResponse = interactionsValue["value"].ToList();

            List<ParentInteraction> interactions = new List<ParentInteraction>();

            foreach (JToken m in interactionsResponse)
            {
                ParentInteraction interaction = new ParentInteraction()
                {
                    Id = m["hackney_tenancymanagementinteractionsid"]?.ToString(),
                    Name = m["hackney_name"]?.ToString(),
                    CreatedOn = m["createdon"]?.ToString(),
                    Contact = m["_hackney_contactid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    NatureofEnquiry = m["hackney_natureofenquiry@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    CreatedByEstateOfficer = m["_hackney_estateofficer_createdbyid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    EnquirySubject = m["hackney_enquirysubject@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    EstateOfficerPatch = m["_hackney_estateofficerpatchid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    HandledBy = m["hackney_handleby@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    HouseholdInteraction = m["_hackney_household_interactionid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    Incident = m["_hackney_incidentid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    ManagerPatch = m["_hackney_managerpropertypatchid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    NHOAreaName = m["hackney_areaname@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    ProcessStage = m["hackney_process_stage@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    ProcessType = m["hackney_processtype@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    ReasonForStartingProcess = m["hackney_reasonforstartingprocess@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    Subject = m["_hackney_subjectid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    Transferred = m["hackney_transferred@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    UpdatedByEstateOfficer = m["_hackney_estateofficer_updatedbyid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                    AddressStreet1 = m["contact1_x002e_address1_line1"]?.ToString(),
                    AddressStreet2 = m["contact1_x002e_address1_line2"]?.ToString(),
                    AddressStreet3 = m["contact1_x002e_address1_line3"]?.ToString(),
                    AddressZIPPostalCode = m["contact1_x002e_address1_postalcode"]?.ToString()
                };

                interactions.Add(interaction);
            }

            //get child interactions
            foreach(var parentInteraction in interactions)
            {
                string childInteractionsQuery =
               $@"
                <fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>
                    <entity name='hackney_tenancymanagementinteractions'>
                    <attribute name='hackney_tenancymanagementinteractionsid'/>
                    <attribute name='hackney_name'/>
                    <attribute name='createdon'/>
                    <attribute name='hackney_estateofficer_updatedbyid'/>
                    <attribute name='hackney_transferred'/>
                    <attribute name='hackney_subjectid'/>
                    <attribute name='hackney_reasonforstartingprocess'/>
                    <attribute name='hackney_processtype'/>
                    <attribute name='hackney_process_stage'/>
                    <attribute name='hackney_areaname'/>
                    <attribute name='hackney_natureofenquiry'/>
                    <attribute name='hackney_managerpropertypatchid'/>
                    <attribute name='hackney_incidentid'/>
                    <attribute name='hackney_household_interactionid'/>
                    <attribute name='hackney_handleby'/>
                    <attribute name='hackney_estateofficerpatchid'/>
                    <attribute name='hackney_enquirysubject'/>
                    <attribute name='hackney_estateofficer_createdbyid'/>
                    <attribute name='hackney_contactid'/>
                    <attribute name='hackney_parent_interactionid'/>
                    <order descending='false' attribute='hackney_name'/>
                    <filter type='and'>
                        <condition attribute='hackney_processtype' value='2' operator='eq'/>
                        <condition attribute='hackney_parent_interactionid' operator='eq' value='{parentInteraction.Id}'/>
                    </filter>
                    <link-entity name='contact' link-type='outer' visible='false' to='hackney_contactid' from='contactid'>
                        <attribute name='address1_postalcode'/>
                        <attribute name='address1_line3'/>
                        <attribute name='address1_line2'/>
                        <attribute name='address1_line1'/>
                    </link-entity>
                    </entity>
                    </fetch>
                ";

                string cUrl = $"{CRMAPIBaseURL}/hackney_tenancymanagementinteractionses?fetchXml={childInteractionsQuery}";

                HttpResponseMessage cresponse = _client.GetAsync(cUrl).Result;
                string cinteractionsResult = cresponse.Content.ReadAsStringAsync().Result;
                var cinteractionsValue = JsonConvert.DeserializeObject<JObject>(cinteractionsResult);
                List<JToken> cinteractionsResponse = cinteractionsValue["value"].ToList();
                
                foreach (JToken m in cinteractionsResponse)
                {
                    Interaction childInteraction = new Interaction()
                    {
                        Id = m["hackney_tenancymanagementinteractionsid"]?.ToString(),
                        Name = m["hackney_name"]?.ToString(),
                        ParentInteractionId = m["_hackney_parent_interactionid_value"]?.ToString(),
                        CreatedOn = m["createdon"]?.ToString(),
                        Contact = m["_hackney_contactid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        NatureofEnquiry = m["hackney_natureofenquiry@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        CreatedByEstateOfficer = m["_hackney_estateofficer_createdbyid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        EnquirySubject = m["hackney_enquirysubject@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        EstateOfficerPatch = m["_hackney_estateofficerpatchid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        HandledBy = m["hackney_handleby@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        HouseholdInteraction = m["_hackney_household_interactionid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        Incident = m["_hackney_incidentid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        ManagerPatch = m["_hackney_managerpropertypatchid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        NHOAreaName = m["hackney_areaname@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        ProcessStage = m["hackney_process_stage@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        ProcessType = m["hackney_processtype@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        ReasonForStartingProcess = m["hackney_reasonforstartingprocess@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        Subject = m["_hackney_subjectid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        Transferred = m["hackney_transferred@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        UpdatedByEstateOfficer = m["_hackney_estateofficer_updatedbyid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                        AddressStreet1 = m["contact1_x002e_address1_line1"]?.ToString(),
                        AddressStreet2 = m["contact1_x002e_address1_line2"]?.ToString(),
                        AddressStreet3 = m["contact1_x002e_address1_line3"]?.ToString(),
                        AddressZIPPostalCode = m["contact1_x002e_address1_postalcode"]?.ToString()
                    };

                    parentInteraction.ChildInteractions.Add(childInteraction);
                }
            }

            return interactions;
        }
    }
}
