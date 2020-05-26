using System.Diagnostics.CodeAnalysis;
using AutoFixture.Xunit2;
using Xunit;

namespace Innergy.Demo.Services.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class InlineAutoMoqDataAttribute : CompositeDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] values)
            : base(new InlineDataAttribute(values), new AutoMoqDataAttribute())
        {
        }
    }
}