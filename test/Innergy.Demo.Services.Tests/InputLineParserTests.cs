using System.Linq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Tests.Infrastructure;
using Moq;
using SemanticComparison.Fluent;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class InputLineParserTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(InputLineParser).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void Parse_ShouldReturnCorrectResult([Frozen] Mock<IInputLineModelBuilder> builderMock,
                                                    string line,
                                                    InputLineModel model,
                                                    InputLineParser sut)
        {
            // arrange
            builderMock.Setup(m => m.Build()).Returns(model);

            // act
            var actual = sut.Parse(line);

            // assert
            model.AsSource()
                 .OfLikeness<InputLineModel>()
                 .With(m => m.Id).EqualsWhen((m, i) => m.Id == i.Id)
                 .With(m => m.Name).EqualsWhen((m, i) => m.Name == i.Name)
                 .With(m => m.Quantities).EqualsWhen((m, i) => m.Quantities.SequenceEqual(i.Quantities))
                 .ShouldEqual(actual);
        }
    }
}