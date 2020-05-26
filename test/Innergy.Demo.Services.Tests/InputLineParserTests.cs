using System.Linq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Tests.Infrastructure;
using Moq;
using SemanticComparison.Fluent;
using Shouldly;
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
        public void Parse_ShouldReturnNullForCommentLine([Frozen] Mock<IInputLineModelBuilder> builderMock, string line, InputLineParser sut)
        {
            // arrange 
            builderMock.Setup(m => m.TryBuildComment(line)).Returns(true);

            // act
            var actual = sut.Parse(line);

            // assert
            actual.ShouldBeNull();
        }

        [Theory, AutoMoqData]
        public void Parse_ShouldReturnCorrectResult([Frozen] Mock<IInputLineModelBuilder> builderMock, 
                                               string line,
                                               InputLineModel model, 
                                               InputLineParser sut)
        {
            // arrange
            builderMock.Setup(m => m.TryBuildComment(line)).Returns(false);
            builderMock.Setup(m => m.TryBuildId(line)).Returns(true);
            builderMock.Setup(m => m.TryBuildName(line)).Returns(true);
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

        [Theory, AutoMoqData]
        public void Parse_ThrowExceptionWhenUnableToParseId([Frozen] Mock<IInputLineModelBuilder> builderMock,
                                                    string line,
                                                    InputLineModel model,
                                                    InputLineParser sut)
        {
            // arrange
            builderMock.Setup(m => m.TryBuildComment(line)).Returns(false);
            builderMock.Setup(m => m.TryBuildId(line)).Returns(false);
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








        [Theory]
        [InlineAutoMoqData("Name;ABC-123;WH-A,1|WH-B,2", "WH-A", 1, "WH-B", 2)]
        public void Parse_ShouldCaptureQuantities(string line, string whName1, int qty1, string whName2, int qty2,
                                                  InputLineParser sut)
        {
            // act
            var actual = sut.Parse(line);

            // assert
            actual.Quantities.ShouldNotBeNull();
            actual.Quantities.Count().ShouldBe(2);
            actual.Quantities.ShouldContain(m => string.Equals(m.WarehouseName, whName1) && m.Quantity == qty1);
            actual.Quantities.ShouldContain(m => string.Equals(m.WarehouseName, whName2) && m.Quantity == qty2);
        }

        [Theory]
        [InlineAutoMoqData("Name;ABC-123;WH-A,1.2")]
        [InlineAutoMoqData("Name;ABC-123;WH-A,-1")]
        public void Parse_ShouldThrowParsingException_WhenQuantityIsNotPositiveInteger(string line, InputLineParser sut)
        {
            // act
            var actual = Record.Exception(() => sut.Parse(line));

            // assert
            actual.ShouldNotBeNull();
            actual.ShouldBeOfType<InputLineParsingException>();
        }

        [Theory]
        [InlineAutoMoqData(";ABC-123;WH-A,1")]
        [InlineAutoMoqData("Name;;WH-A,1")]
        public void Parse_ShouldThrowParsingException_WhenNameOrIdIsEmpty(string line, InputLineParser sut)
        {
            // act
            var actual = Record.Exception(() => sut.Parse(line));

            // assert
            actual.ShouldNotBeNull();
            actual.ShouldBeOfType<InputLineParsingException>();
        }
    }
}