using Bogus;
using MaTReportingAPI.V1.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MaTReportingAPI.Tests.V1.Helpers
{
    public static class InteractionsHelper
    {
        private static readonly Faker faker = new Faker();
        public static string GetExpectedCRMGatewayResponseForGetInteractions()
        {
            return $@"
                {{
                    ""@odata.context"":""https://lbhackneydev.crm11.dynamics.com/api/data/v8.2/$metadata#hackney_tenancymanagementinteractionses(_hackney_incidentid_value,hackney_incidentId,createdon,_hackney_estateofficer_createdbyid_value,hackney_estateofficer_createdbyid,hackney_natureofenquiry,hackney_areaname,_hackney_contactid_value,hackney_contactId,hackney_tenancymanagementinteractionsid,_hackney_estateofficerpatchid_value,hackney_estateofficerpatchid,hackney_enquirysubject,hackney_transferred,hackney_name,hackney_reasonforstartingprocess,hackney_handleby,_hackney_household_interactionid_value,hackney_household_Interactionid,hackney_process_stage,_hackney_subjectid_value,hackney_subjectId,hackney_processtype,hackney_estateofficer_updatedbyid,hackney_managerpropertypatchid,hackney_incidentId(),hackney_estateofficer_createdbyid(),hackney_contactId(),hackney_estateofficerpatchid(),hackney_household_Interactionid(),hackney_subjectId(),hackney_estateofficer_updatedbyid(),hackney_managerpropertypatchid())"",
                ""value"":[
                {{""@odata.etag"":""W/\""49208235\"""",
                ""_hackney_incidentid_value@OData.Community.Display.V1.FormattedValue"":""Tenancy Management"",
                ""_hackney_incidentid_value"":""{Guid.NewGuid().ToString()}"",
                ""createdon@OData.Community.Display.V1.FormattedValue"":""02/04/2019 09:33"",
                ""createdon"":""2019-04-02T09:33:03Z"",
                ""_hackney_estateofficer_createdbyid_value@OData.Community.Display.V1.FormattedValue"":""{faker.Name}"",
                ""_hackney_estateofficer_createdbyid_value"":""{Guid.NewGuid().ToString()}"",
                ""hackney_natureofenquiry@OData.Community.Display.V1.FormattedValue"":""Tenancy Audit and Visits"",
                ""hackney_natureofenquiry"":15,
                ""hackney_areaname@OData.Community.Display.V1.FormattedValue"":""Central"",
                ""hackney_areaname"":1,
                ""_hackney_contactid_value@OData.Community.Display.V1.FormattedValue"":""{faker.Name}"",
                ""_hackney_contactid_value"":""{Guid.NewGuid().ToString()}"",
                ""hackney_tenancymanagementinteractionsid"":""{Guid.NewGuid().ToString()}"",
                ""_hackney_estateofficerpatchid_value@OData.Community.Display.V1.FormattedValue"":""SH2"",
                ""_hackney_estateofficerpatchid_value"":""{Guid.NewGuid().ToString()}"",
                ""hackney_enquirysubject@OData.Community.Display.V1.FormattedValue"":""Tenancy and household check"",
                ""hackney_enquirysubject"":100000156,
                ""hackney_transferred@OData.Community.Display.V1.FormattedValue"":""No"",
                ""hackney_transferred"":false,
                ""hackney_name"":""{faker.Random.Word()}"",
                ""hackney_reasonforstartingprocess@OData.Community.Display.V1.FormattedValue"":""{faker.Random.Word()}"",
                ""hackney_reasonforstartingprocess"":1,
                ""hackney_handleby@OData.Community.Display.V1.FormattedValue"":""Estate Officer"",
                ""hackney_handleby"":1,
                ""_hackney_household_interactionid_value@OData.Community.Display.V1.FormattedValue"":""000633/01"",
                ""_hackney_household_interactionid_value"":""{Guid.NewGuid().ToString()}"",
                ""hackney_process_stage@OData.Community.Display.V1.FormattedValue"":""Not completed"",
                ""hackney_process_stage"":0,
                ""_hackney_subjectid_value@OData.Community.Display.V1.FormattedValue"":""Tenancy Management Interactions"",
                ""_hackney_subjectid_value"":""{Guid.NewGuid().ToString()}"",
                ""hackney_processtype@OData.Community.Display.V1.FormattedValue"":""Process"",
                ""hackney_processtype"":1,
                ""contact1_x002e_address1_postalcode"":""{faker.Random.Word()}"",
                ""contact1_x002e_address1_line2"":""{faker.Random.Word()}"",
                ""contact1_x002e_address1_line1"":""{faker.Random.Word()}"",
                ""contact1_x002e_address1_line3"":""HACKNEY""
                }},

                {{""@odata.etag"":""W/\""49208518\"""",
                 ""_hackney_incidentid_value@OData.Community.Display.V1.FormattedValue"":""Tenancy Management"",
                ""_hackney_incidentid_value"":""{Guid.NewGuid().ToString()}"",
                ""createdon@OData.Community.Display.V1.FormattedValue"":""02/04/2019 09:33"",
                ""createdon"":""2019-04-02T09:33:03Z"",
                ""_hackney_estateofficer_createdbyid_value@OData.Community.Display.V1.FormattedValue"":""{faker.Name}"",
                ""_hackney_estateofficer_createdbyid_value"":""{Guid.NewGuid().ToString()}"",
                ""hackney_natureofenquiry@OData.Community.Display.V1.FormattedValue"":""Tenancy Audit and Visits"",
                ""hackney_natureofenquiry"":15,
                ""hackney_areaname@OData.Community.Display.V1.FormattedValue"":""Central"",
                ""hackney_areaname"":1,
                ""_hackney_contactid_value@OData.Community.Display.V1.FormattedValue"":""{faker.Name}"",
                ""_hackney_contactid_value"":""{Guid.NewGuid().ToString()}"",
                ""hackney_tenancymanagementinteractionsid"":""{Guid.NewGuid().ToString()}"",
                ""_hackney_estateofficerpatchid_value@OData.Community.Display.V1.FormattedValue"":""SH2"",
                ""_hackney_estateofficerpatchid_value"":""{Guid.NewGuid().ToString()}"",
                ""hackney_enquirysubject@OData.Community.Display.V1.FormattedValue"":""Tenancy and household check"",
                ""hackney_enquirysubject"":100000156,
                ""hackney_transferred@OData.Community.Display.V1.FormattedValue"":""No"",
                ""hackney_transferred"":false,
                ""hackney_name"":""{faker.Random.Word()}"",
                ""hackney_reasonforstartingprocess@OData.Community.Display.V1.FormattedValue"":""{faker.Random.Word()}"",
                ""hackney_reasonforstartingprocess"":1,
                ""hackney_handleby@OData.Community.Display.V1.FormattedValue"":""Estate Officer"",
                ""hackney_handleby"":1,
                ""_hackney_household_interactionid_value@OData.Community.Display.V1.FormattedValue"":""000633/01"",
                ""_hackney_household_interactionid_value"":""{Guid.NewGuid().ToString()}"",
                ""hackney_process_stage@OData.Community.Display.V1.FormattedValue"":""Not completed"",
                ""hackney_process_stage"":0,
                ""_hackney_subjectid_value@OData.Community.Display.V1.FormattedValue"":""Tenancy Management Interactions"",
                ""_hackney_subjectid_value"":""{Guid.NewGuid().ToString()}"",
                ""hackney_processtype@OData.Community.Display.V1.FormattedValue"":""Process"",
                ""hackney_processtype"":1,
                ""contact1_x002e_address1_postalcode"":""{faker.Random.Word()}"",
                ""contact1_x002e_address1_line2"":""{faker.Random.Word()}"",
                ""contact1_x002e_address1_line1"":""{faker.Random.Word()}"",
                ""contact1_x002e_address1_line3"":""HACKNEY""}}
            ]}}";
        }

        public static string GetHomeCheckResults()
        {
            return
            $@"
                {{
                    ""{Guid.NewGuid().ToString()}"": ""No"",
                    ""{Guid.NewGuid().ToString()}"": ""Yes""
                }}
            ";
        }

        public static Dictionary<string, object> GetInteractionsAsDictionaryObject()
        {
            return new Dictionary<string, object>
                {
                    {"childInteractions", new [] {
                        new Dictionary<string, object>
                            {
                                {"childInteractions", Array.Empty<Interaction>() },
                                {"id","a4b44f8e-e714-ea11-a811-000d3a86d68d"},
                                {"parentInteractionId", "02d0ff4d-2555-e911-a555-002248072xyz" },
                                {"name","CASE-NAME"},
                                {"createdOn","24/12/2019"},
                                {"contact","Contact name"},
                                {"natureofEnquiry","nature of enquiry"},
                                {"createdByEstateOfficer","Created by estate officer"},
                                {"enquirySubject","enquiry subject"},
                                {"estateOfficerPatch","estate officer patch"},
                                {"handledBy","Handled Officer"},
                                {"householdInteraction","555"},
                                {"incident","incident type"},
                                {"managerPatch","manager patch"},
                                {"nhoAreaName","NHO area name"},
                                {"processStage","555"},
                                {"processType","555"},
                                {"reasonForStartingProcess","reason for processing"},
                                {"subject","subject"},
                                {"transferred","transferred"},
                                {"updatedByEstateOfficer","updated by"},
                                {"addressStreet1","Address Street 1"},
                                {"addressStreet2","Address Street 2"},
                                {"addressStreet3","Address Street 3"},
                                {"addressZIPPostalCode","E18"},
                                {"homeCheck","home check"}
                            }
                        }
                    },
                    {"id","02d0ff4d-2555-e911-a555-002248072xyz"},
                    {"parentInteractionId", null },
                    {"name","CASE-NAME"},
                    {"createdOn","24/12/2019"},
                    {"contact","Contact name"},
                    {"natureofEnquiry","nature of enquiry"},
                    {"createdByEstateOfficer","Created by estate officer"},
                    {"enquirySubject","enquiry subject"},
                    {"estateOfficerPatch","estate officer patch"},
                    {"handledBy","Handled Officer"},
                    {"householdInteraction","555"},
                    {"incident","incident type"},
                    {"managerPatch","manager patch"},
                    {"nhoAreaName","NHO area name"},
                    {"processStage","555"},
                    {"processType","555"},
                    {"reasonForStartingProcess","reason for processing"},
                    {"subject","subject"},
                    {"transferred","transferred"},
                    {"updatedByEstateOfficer","updated by"},
                    {"addressStreet1","Address Street 1"},
                    {"addressStreet2","Address Street 2"},
                    {"addressStreet3","Address Street 3"},
                    {"addressZIPPostalCode","E18"},
                    {"homeCheck","home check"}
                };
        }

        public static Interaction GetInteraction()
        {
            Interaction interaction = new Interaction()
            {
                AddressStreet1 = "Address Street 1",
                AddressStreet2 = "Address Street 2",
                AddressStreet3 = "Address Street 3",
                AddressZIPPostalCode = "E18",
                Contact = "Contact name",
                CreatedByEstateOfficer = "Created by estate officer",
                CreatedOn = "24/12/2019",
                EnquirySubject = "enquiry subject",
                EstateOfficerPatch = "estate officer patch",
                HandledBy = "Handled Officer",
                HomeCheck = "home check",
                HouseholdInteraction = "555",
                Id = "a4b44f8e-e714-ea11-a811-000d3a86d68d",
                Incident = "incident type",
                ManagerPatch = "manager patch",
                Name = "CASE-NAME",
                NatureofEnquiry = "nature of enquiry",
                NHOAreaName = "NHO area name",
                ProcessStage = "555",
                ProcessType = "555",
                ReasonForStartingProcess = "reason for processing",
                Subject = "subject",
                Transferred = "transferred",
                UpdatedByEstateOfficer = "updated by",
                ParentInteractionId = "02d0ff4d-2555-e911-a555-002248072xyz"
            };

            return new Interaction()
            {
                AddressStreet1 = "Address Street 1",
                AddressStreet2 = "Address Street 2",
                AddressStreet3 = "Address Street 3",
                AddressZIPPostalCode = "E18",
                Contact = "Contact name",
                CreatedByEstateOfficer = "Created by estate officer",
                CreatedOn = "24/12/2019",
                EnquirySubject = "enquiry subject",
                EstateOfficerPatch = "estate officer patch",
                HandledBy = "Handled Officer",
                HomeCheck = "home check",
                HouseholdInteraction = "555",
                Id = "02d0ff4d-2555-e911-a555-002248072xyz",
                Incident = "incident type",
                ManagerPatch = "manager patch",
                Name = "CASE-NAME",
                NatureofEnquiry = "nature of enquiry",
                NHOAreaName = "NHO area name",
                ProcessStage = "555",
                ProcessType = "555",
                ReasonForStartingProcess = "reason for processing",
                Subject = "subject",
                Transferred = "transferred",
                UpdatedByEstateOfficer = "updated by",
                ParentInteractionId = null,
                ChildInteractions = new List<Interaction>() { interaction }
            };
        }
    }
}
