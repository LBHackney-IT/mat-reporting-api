namespace MaTReportingAPI.V1.Domain
{
    public class Interaction
    {
        public string Id { get; set; }
        public string ParentInteractionId { get; set; }
        public string Name { get; set; }
        public string CreatedOn  { get; set; }
        public string Contact { get; set; }
        public string NatureofEnquiry { get; set; }
        public string CreatedByEstateOfficer { get; set; }
        public string EnquirySubject { get; set; }
        public string EstateOfficerPatch { get; set; }
        public string HandledBy { get; set; }
        public string HouseholdInteraction { get; set; }
        public string Incident { get; set; }
        public string ManagerPatch { get; set; }
        public string NHOAreaName { get; set; }
        public string ProcessStage { get; set; }
        public string ProcessType { get; set; }
        public string ReasonForStartingProcess { get; set; }
        public string Subject { get; set; }
        public string Transferred { get; set; }
        public string UpdatedByEstateOfficer { get; set; }
        public string AddressStreet1 { get; set; }
        public string AddressStreet2 { get; set; }
        public string AddressStreet3  { get; set; }
        public string AddressZIPPostalCode { get; set; }
        public string HomeCheck { get; set; }
    }
}
