using System.IO;
using AutoFixture;

namespace Innergy.Demo.Services.Tests.Infrastructure
{
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