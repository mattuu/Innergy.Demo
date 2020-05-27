using System;
using System.IO;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Input;
using Innergy.Demo.Services.Tests.Infrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace Innergy.Demo.Services.Tests.Input
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
        public void Load_ShouldReturnCorrectResult(IFixture fixture, [Frozen] Mock<IInputLineParser> inputLineParserMock, string line, InputLineModel model)
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

            fixture.Customize(new TextFileInputStrategyCustomization(filePath));

            var sut = fixture.Create<TextFileInputStrategy>();

            // act
            sut.Load();

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