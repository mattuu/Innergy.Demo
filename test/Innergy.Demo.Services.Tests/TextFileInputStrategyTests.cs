using System;
using System.IO;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Tests.Infrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class TextFileInputStrategyTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(TextFileInputStrategy).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void Load_ShouldReturnCorrectResult([Frozen] Mock<IInputLineParser> inputLineParserMock, string line, InputLineModel model, TextFileInputStrategy sut)
        {
            // arrange
            inputLineParserMock.Setup(m => m.Parse(line)).Returns(model);

            var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.tmp");
            using (var file = File.OpenWrite(filePath))
            { 
                using(var streamWriter = new StreamWriter(file))
                {
                    streamWriter.WriteLine(line);
                }
            }

            // act
            sut.Load(filePath);

            // assert
            var actual = sut.GetModels();
            actual.ShouldHaveSingleItem().ShouldBe(model);

            // teardown
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}