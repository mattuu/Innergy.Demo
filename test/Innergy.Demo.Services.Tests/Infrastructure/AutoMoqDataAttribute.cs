using System.Diagnostics.CodeAnalysis;
using System.IO;
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


    public class MemoryStreamCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() => new MemoryStream());
            fixture.Register(() => new StreamReader(new MemoryStream()));
            fixture.Register(() => new StreamReader(new MemoryStream()));
        }
    }
}