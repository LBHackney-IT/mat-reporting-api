using MaTReportingAPI.V1.Domain;
using Xunit;

namespace MaTReportingAPI.Tests.V1.Domain
{
    public class ETRAMeetingTests
    {
        [Fact]
        public void ETRAMeetingsHaveName()
        {
            ETRAMeeting meeting = new ETRAMeeting();
            Assert.Null(meeting.Name);
        }
    }
}

