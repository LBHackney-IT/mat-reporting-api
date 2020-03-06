using MaTReportingAPI.V1.Domain;
using Newtonsoft.Json.Linq;

namespace MaTReportingAPI.V1.Factories
{
    public static class CRMEntityFactory
    {
        public static Interaction CreateInteractionObject(JToken token)
        {
            return new Interaction()
            {
                Id = token["hackney_tenancymanagementinteractionsid"]?.ToString(),
                Name = token["hackney_name"]?.ToString(),
                CreatedOn = token["createdon"]?.ToString(),
                Contact = token["_hackney_contactid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                NatureofEnquiry = token["hackney_natureofenquiry@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                CreatedByEstateOfficer = token["_hackney_estateofficer_createdbyid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                EnquirySubject = token["hackney_enquirysubject@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                EstateOfficerPatch = token["_hackney_estateofficerpatchid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                HandledBy = token["hackney_handleby@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                HouseholdInteraction = token["_hackney_household_interactionid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                Incident = token["_hackney_incidentid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                ManagerPatch = token["_hackney_managerpropertypatchid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                NHOAreaName = token["hackney_areaname@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                ProcessStage = token["hackney_process_stage@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                ProcessType = token["hackney_processtype@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                ReasonForStartingProcess = token["hackney_reasonforstartingprocess@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                Subject = token["_hackney_subjectid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                Transferred = token["hackney_transferred@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                UpdatedByEstateOfficer = token["_hackney_estateofficer_updatedbyid_value@OData.Community.Display.V1.FormattedValue"]?.ToString(),
                AddressStreet1 = token["contact1_x002e_address1_line1"]?.ToString(),
                AddressStreet2 = token["contact1_x002e_address1_line2"]?.ToString(),
                AddressStreet3 = token["contact1_x002e_address1_line3"]?.ToString(),
                AddressZIPPostalCode = token["contact1_x002e_address1_postalcode"]?.ToString(),
                ParentInteractionId = token["hackney_parent_interactionid"]?.ToString()
            };
        }
    }
}
