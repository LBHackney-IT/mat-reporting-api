using MaTReportingAPI.V1.Domain;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Domain
{
    public class InteractionTests
    {
        [Fact]
        public void InteractionsHaveId()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.Id);
        }

        [Fact]
        public void InteractionsHaveParentInteractionId()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.ParentInteractionId);
        }

        [Fact]
        public void InteractionsHaveName()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.Name);
        }

        [Fact]
        public void InteractionsHaveCreatedOn()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.CreatedOn);
        }

        [Fact]
        public void InteractionsHaveContact()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.Contact);
        }

        [Fact]
        public void InteractionsHaveCreatedByEstateOfficer()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.CreatedByEstateOfficer);
        }

        [Fact]
        public void InteractionsHaveNatureOfEnquiry()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.NatureofEnquiry);
        }

        [Fact]
        public void InteractionsHaveEstateOfficerPatch()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.EstateOfficerPatch);
        }

        [Fact]
        public void InteractionsHaveEnquirySubject()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.EnquirySubject);
        }

        [Fact]
        public void InteractionsHaveHandleBy()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.HandledBy);
        }

        [Fact]
        public void InteractionsHaveHouseholdInteraction()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.HouseholdInteraction);
        }

        [Fact]
        public void InteractionsHaveIncident()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.Incident);
        }

        [Fact]
        public void InteractionsHaveManagerPatch()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.ManagerPatch);
        }

        [Fact]
        public void InteractionsHaveNHOAreaName()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.NHOAreaName);
        }

        [Fact]
        public void InteractionsHaveProcessStage()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.ProcessStage);
        }

        [Fact]
        public void InteractionsHaveProcessType()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.ProcessType);
        }

        [Fact]
        public void InteractionsHaveResonForStartingProcess()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.ReasonForStartingProcess);
        }

        [Fact]
        public void InteractionsHaveSubject()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.Subject);
        }

        [Fact]
        public void InteractionsHaveTransferred()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.Transferred);
        }

        [Fact]
        public void InteractionsHaveUpdatedByEstateOfficer()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.UpdatedByEstateOfficer);
        }

        [Fact]
        public void InteractionsHaveAddressStreet1()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.AddressStreet1);
        }

        [Fact]
        public void InteractionsHaveAddressStreet2()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.AddressStreet2);
        }

        [Fact]
        public void InteractionsHaveAddressStreet3()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.AddressStreet3);
        }

        [Fact]
        public void InteractionsHaveAddressZIOPostalCode()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.AddressZIPPostalCode);
        }


        [Fact]
        public void InteractionsHaveHomeCheck()
        {
            Interaction interaction = new Interaction();
            Assert.Null(interaction.HomeCheck);
        }

        [Fact]
        public void ParentInteractionsHaveListOfChildInteractions()
        {
            ParentInteraction parentInteraction = new ParentInteraction();
            Assert.Empty(parentInteraction.ChildInteractions);
        }
    }
}
