using MaTReportingAPI.UseCase.V1;
using Xunit;

namespace UnitTests.V1.UseCase
{
    public class ThrowOpsErrorUsecaseTests
    {
        [Fact]
        public void ThrowsTestOpsErrorException()
        {
            TestOpsErrorException ex = Assert.Throws<TestOpsErrorException>(
                delegate { ThrowOpsErrorUsecase.Execute(); });

            Assert.Equal("This is a test exception to test our integrations", ex.Message);
        }
    }
}
