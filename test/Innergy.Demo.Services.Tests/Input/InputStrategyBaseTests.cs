using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Input;
using Innergy.Demo.Services.Tests.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace Innergy.Demo.Services.Tests.Input
{
    public class InputStrategyBaseTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(InputStrategyBase).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void GetModels_ShouldReturnCorrectResult([Frozen] Mock<IInputLineParser> inputLineParserMock,
                                                        string line,
                                                        InputLineModel model,
                                                        TestInputStrategy sut)
        {
            // arrange
            inputLineParserMock.Setup(m => m.Parse(line)).Returns(model);
            sut.ParseLine(line);

            // act
            var actual = sut.GetModels();

            // assert
            actual.ShouldHaveSingleItem().ShouldBe(model);
        }

        public class TestInputStrategy : InputStrategyBase
        {
            public TestInputStrategy(ILogger<InputStrategyBase> logger, IInputLineParser inputLineParser)
                : base(logger, inputLineParser)
            {
            }
        }
    }
}