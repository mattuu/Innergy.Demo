using System.Linq;
using AutoFixture.Idioms;
using Innergy.Demo.Services.Tests.Infrastructure;
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
        public void Parse_ShouldReturnNullForCommentLine(string line, InputLineParser sut)
        {
            // arrange 
            var commentLine = $"{InputLineParser.CommentPrefix}{line}";

            // act
            var actual = sut.Parse(commentLine);

            // assert
            actual.ShouldBeNull();
        }

        [Theory]
        [InlineAutoMoqData("ABC-123;foo", "ABC-123")]
        [InlineAutoMoqData("foo;ABC-123", "ABC-123")]
        public void Parse_ShouldCaptureIdField(string line, string expected, InputLineParser sut)
        {
            // act
            var actual = sut.Parse(line);

            // assert
            actual.Id.ShouldBe(expected);
        }

        [Theory]
        [InlineAutoMoqData("This is a name;ABC-123", "This is a name")]
        [InlineAutoMoqData("ABC-123;This is a name", "This is a name")]
        public void Parse_ShouldCaptureNameField(string line, string expected, InputLineParser sut)
        {
            // act
            var actual = sut.Parse(line);

            // assert
            actual.Name.ShouldBe(expected);
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