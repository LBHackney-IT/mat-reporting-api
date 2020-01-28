using System.Collections.Generic;

namespace MaTReportingAPI.V1.Domain
{
    public class ParentInteraction : Interaction
    {
        public List<Interaction> ChildInteractions { get; set; } = new List<Interaction>();
    }
}
