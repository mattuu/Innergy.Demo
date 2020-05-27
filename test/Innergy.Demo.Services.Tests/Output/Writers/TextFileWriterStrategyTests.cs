using System;
using System.IO;
using AutoFixture.Idioms;
using Innergy.Demo.Services.Output.Writers;
using Innergy.Demo.Services.Tests.Infrastructure;
using Shouldly;
using Xunit;

namespace Innergy.Demo.Services.Tests.Output.Writers
{
    public class TextFileWriterStrategyTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(TextFileWriterStrategy).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void WriteLine_ShouldWriteToFile(string line)
        {
            // arrange
            var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.tmp");

            using (var sut = new TextFileWriterStrategy(filePath))
            {
                // act
                sut.WriteLine(line);
            }
                
            // assert
            File.Exists(filePath).ShouldBeTrue();
            using (var reader = File.OpenText(filePath))
            {
                reader.ReadLine().ShouldBe(line);
            }

            // teardown
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}