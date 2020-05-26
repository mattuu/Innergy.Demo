using AutoFixture.Idioms;
using Innergy.Demo.Services.Tests.Infrastructure;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class JobRunnerTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(JobRunner).GetConstructors());
        }
    }
}