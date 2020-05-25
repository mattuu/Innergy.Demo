using AutoFixture.Idioms;
using Solex.DevTest.TestUtils;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class DataProcessorTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(DataProcessor).GetConstructors());
        }
    }
}