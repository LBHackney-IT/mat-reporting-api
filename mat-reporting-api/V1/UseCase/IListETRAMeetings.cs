using System.Threading.Tasks;

namespace MaTReportingAPI.V1.Boundary
{
    public interface IListETRAMeetings
    {
        ListETRAMeetingsResponse Execute(ListETRAMeetingsRequest request);
    }
}
