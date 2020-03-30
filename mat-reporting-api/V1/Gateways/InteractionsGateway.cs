using MaTReportingAPI.V1.Domain;
using MaTReportingAPI.V1.Factories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaTReportingAPI.V1.Gateways
{
    public class InteractionsGateway : IInteractionsGateway
    {
        private readonly ICRMGateway _CRMGateway;
        private readonly IMaTProcessAPIGateway _MaTProcessAPIGateway;
        private readonly IProcessDataGateway _processDataGateway;
        private readonly string enableDocumentDBSupport;

        public InteractionsGateway(ICRMGateway CRMGateway, IMaTProcessAPIGateway MaTProcessAPI, IProcessDataGateway processDataGateway)
        {
            _CRMGateway = CRMGateway;
            _MaTProcessAPIGateway = MaTProcessAPI;
            _processDataGateway = processDataGateway;
            enableDocumentDBSupport = Environment.GetEnvironmentVariable("EnableDocumentDBSupport") != null
                ? Environment.GetEnvironmentVariable("EnableDocumentDBSupport").ToString().ToLower() : "false";
        }

        public List<Interaction> GetInteractionsByDateRange(string fromDate, string toDate)
        {
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
                    <condition attribute='hackney_natureofenquiry' value='15' operator='eq'/>
                    <condition attribute='hackney_enquirysubject' operator='in'>
                        <value>100000156</value>
                        <value>100000052</value>
                        <value>100000060</value>
                    </condition>
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

            dynamic interactionsResponse = _CRMGateway.GetEntitiesByFetchXMLQuery("hackney_tenancymanagementinteractionses", interactionsQuery);

            List<Interaction> interactions = new List<Interaction>();

            foreach (JToken m in interactionsResponse)
            {
                interactions.Add(CRMEntityFactory.CreateInteractionObject(m));
            }
            //only get Tenancy and Household Check (THC) interactions
            List<string> interactionIDs = interactions.Where(x => x.EnquirySubject == "Tenancy and household check").Select(x => x.Id).ToList();
           
            //get home check data for processes created using Angular UI
            var homeChecks = _MaTProcessAPIGateway.GetHomeCheckAnswersByInteractionIDs(interactionIDs);

            foreach (KeyValuePair<string, JToken> homeCheck in homeChecks)
            {
                Interaction interaction = interactions.FirstOrDefault(x => x.Id == homeCheck.Key);
                if (interaction != null) interaction.HomeCheck = homeCheck.Value?.ToString();
            }

            //check if we need to fetch data for replatformed THC process as well
            if (enableDocumentDBSupport == "true")
            {
                //get home check data for processes created using replatformed React UI
                List<MatProcessData> replatformedHomeChecks = new List<MatProcessData>();

                if (interactionIDs.Count > 0)
                {
                    replatformedHomeChecks = _processDataGateway.GetProcessDataByIDs(interactionIDs.ToArray());

                    //if the process wasn't created using Angular, the API call _MaTProcessAPIGateway.GetHomeCheckAnswersByInteractionIDs above would have returned "no" for that process ID.
                    //This will override those values since they exist in the new process database
                    foreach (var homeCheck in replatformedHomeChecks)
                    {
                        Interaction interaction = interactions.FirstOrDefault(x => x.Id == homeCheck.Id);
                        if (interaction != null)
                        {
                            var processData = ((dynamic)homeCheck.ProcessData);

                            //ensure we have process data, stub records won't have it
                            if (processData != null) interaction.HomeCheck = processData.homeCheck.value;
                        }
                    }
                }
            }

            return interactions;
        }

        public List<Interaction> GetInteractionsAndChildInteractionsByDateRange(string fromDate, string toDate)
        {
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

            dynamic interactionsResponse = _CRMGateway.GetEntitiesByFetchXMLQuery("hackney_tenancymanagementinteractionses", parentInteractionsQuery);

            List<Interaction> interactions = new List<Interaction>();

            foreach (JToken m in interactionsResponse)
            {
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

            //TODO: limit homecheck to THC, use base class and different interaction types
            List<string> interactionIDs = interactions.Select(x => x.Id).ToList();

            var homeChecks = _MaTProcessAPIGateway.GetHomeCheckAnswersByInteractionIDs(interactionIDs);

            foreach (KeyValuePair<string, JToken> homeCheck in homeChecks)
            {
                Interaction interaction = interactions.FirstOrDefault(x => x.Id == homeCheck.Key);
                interaction.HomeCheck = homeCheck.Value?.ToString();
            }

            //get child interactions
            foreach (var parentInteraction in interactions)
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

                dynamic cinteractionsResponse = _CRMGateway.GetEntitiesByFetchXMLQuery("hackney_tenancymanagementinteractionses", childInteractionsQuery);

                foreach (JToken m in cinteractionsResponse)
                {
                    parentInteraction.ChildInteractions.Add(CRMEntityFactory.CreateInteractionObject(m));
                }
            }

            return interactions;
        }
    }
}
