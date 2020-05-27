using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Innergy.Demo.Services.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(CreateFixture)
        {
        }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Customize(new MemoryStreamCustomization())
                   .Customize(new AutoMoqCustomization())
                   .Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
    }
}