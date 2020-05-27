using System.IO;
using AutoFixture.Idioms;
using Innergy.Demo.Domain;
using Innergy.Demo.Services.Parsing;
using Innergy.Demo.Services.Tests.Infrastructure;
using Shouldly;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class TokenizerTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(Tokenizer).GetConstructors());
        }

        [Theory]
        [InlineAutoMoqData("Name")]
        public void Tokenizer_ShouldCaptureProductName(string line)
        {
            // arrange
            using (var stringReader = new StringReader(line))
            {
                // act
                var actual = new Tokenizer(stringReader);

                // assert
                actual.Token.ShouldBe(Token.ProductName);
                actual.Value.ShouldBe("Name");
            }
        }

        [Theory]
        [InlineAutoMoqData("ABC-123")]
        public void Tokenizer_ShouldCaptureProductId(string line)
        {
            // arrange
            using (var stringReader = new StringReader(line))
            {
                // act
                var actual = new Tokenizer(stringReader);

                // assert
                actual.Token.ShouldBe(Token.ProductId);
                actual.Value.ShouldBe("ABC-123");
            }
        }

        [Theory]
        [InlineAutoMoqData("WH-A,1|WHB-2")]
        public void Tokenizer_ShouldCaptureWarehouseInfo(string line)
        {
            // arrange
            using (var stringReader = new StringReader(line))
            {
                // act
                var actual = new Tokenizer(stringReader);

                // assert
                actual.Token.ShouldBe(Token.WarehouseInfo);
            }
        }

        [Theory, AutoMoqData]
        public void Tokenizer_ShouldCaptureComment(string line)
        {
            // arrange
            using (var stringReader = new StringReader($"#{line}"))
            {
                // act
                var actual = new Tokenizer(stringReader);

                // assert
                actual.Token.ShouldBe(Token.Comment);
            }
        }
    }
}

    