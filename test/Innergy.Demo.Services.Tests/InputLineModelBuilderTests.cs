using System.Linq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Innergy.Demo.Domain;
using Innergy.Demo.Services.Tests.Infrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class InputLineModelBuilderTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(InputLineModelBuilder).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void BuildComment_ShouldSetIsCommentToTrueForCommentLine([Frozen] Mock<ITokenizer> tokenizerMock,
                                                                        InputLineModelBuilder sut)
        {
            // arrange 
            tokenizerMock.Setup(m => m.Token).Returns(Token.Comment);

            // act
            sut.BuildComment(tokenizerMock.Object);

            // assert
            sut.IsComment.ShouldBeTrue();
        }

        [Theory]
        [InlineAutoMoqData(Token.EOL)]
        [InlineAutoMoqData(Token.ProductName)]
        [InlineAutoMoqData(Token.ProductId)]
        [InlineAutoMoqData(Token.WarehouseInfo)]
        public void BuildComment_ShouldSetIsCommentToFalseForNonCommentLine(
            Token token, [Frozen] Mock<ITokenizer> tokenizerMock, InputLineModelBuilder sut)
        {
            // arrange 
            tokenizerMock.Setup(m => m.Token).Returns(token);

            // act
            sut.BuildComment(tokenizerMock.Object);

            // assert
            sut.IsComment.ShouldBeFalse();
        }

        [Theory, AutoMoqData]
        public void BuildId_ShouldSetIdFieldFromProductIdToken([Frozen] Mock<ITokenizer> tokenizerMock,
                                                               InputLineModelBuilder sut)
        {
            // arrange 
            tokenizerMock.Setup(m => m.Token).Returns(Token.ProductId);

            // act
            sut.BuildId(tokenizerMock.Object);

            // assert
            sut.Build().Id.ShouldBe(tokenizerMock.Object.Value);
        }

        [Theory]
        [InlineAutoMoqData(Token.EOL)]
        [InlineAutoMoqData(Token.ProductName)]
        [InlineAutoMoqData(Token.Comment)]
        [InlineAutoMoqData(Token.WarehouseInfo)]
        public void BuildId_ShouldNotSetIdFieldFromNonProductIdToken(Token token,
                                                                     [Frozen] Mock<ITokenizer> tokenizerMock,
                                                                     InputLineModelBuilder sut)
        {
            // arrange 
            tokenizerMock.Setup(m => m.Token).Returns(token);

            // act
            sut.BuildId(tokenizerMock.Object);

            // assert
            sut.Build().Id.ShouldBeNull();
        }

        [Theory, AutoMoqData]
        public void BuildName_ShouldSetNameFieldFromProductNameToken([Frozen] Mock<ITokenizer> tokenizerMock,
                                                                     InputLineModelBuilder sut)
        {
            // arrange 
            tokenizerMock.Setup(m => m.Token).Returns(Token.ProductName);

            // act
            sut.BuildId(tokenizerMock.Object);

            // assert
            sut.Build().Name.ShouldBe(tokenizerMock.Object.Value);
        }

        [Theory]
        [InlineAutoMoqData(Token.EOL)]
        [InlineAutoMoqData(Token.ProductId)]
        [InlineAutoMoqData(Token.Comment)]
        [InlineAutoMoqData(Token.WarehouseInfo)]
        public void BuildName_ShouldNotSetNameFieldFromNonProductIdToken(
            Token token, [Frozen] Mock<ITokenizer> tokenizerMock, InputLineModelBuilder sut)
        {
            // arrange 
            tokenizerMock.Setup(m => m.Token).Returns(token);

            // act
            sut.BuildId(tokenizerMock.Object);

            // assert
            sut.Build().Name.ShouldBeNull();
        }

        [Theory]
        [InlineAutoMoqData("WH-A,12|WH-B,3")]
        public void BuildQuantities_ShouldSetQuantitiesFieldWarehouseInfoToken(
            string warehouseInfo, [Frozen] Mock<ITokenizer> tokenizerMock, InputLineModelBuilder sut)
        {
            // arrange 
            tokenizerMock.Setup(m => m.Token).Returns(Token.WarehouseInfo);
            tokenizerMock.Setup(m => m.Value).Returns(warehouseInfo);

            // act
            sut.BuildQuantities(tokenizerMock.Object);

            // assert
            var quantities = sut.Build().Quantities;
            quantities.Count().ShouldBe(2);
            quantities.ShouldContain(q => "WH-A".Equals(q.WarehouseName) && q.Quantity == 12);
            quantities.ShouldContain(q => "WH-B".Equals(q.WarehouseName) && q.Quantity == 3);
        }

        [Theory]
        [InlineAutoMoqData(Token.EOL)]
        [InlineAutoMoqData(Token.ProductId)]
        [InlineAutoMoqData(Token.ProductName)]
        [InlineAutoMoqData(Token.Comment)]
        public void Quantities_ShouldNotSetQuantitiesFieldFromNonWarehouseInfoToken(
            Token token, [Frozen] Mock<ITokenizer> tokenizerMock, InputLineModelBuilder sut)
        {
            // arrange 
            tokenizerMock.Setup(m => m.Token).Returns(token);

            // act
            sut.BuildId(tokenizerMock.Object);

            // assert
            sut.Build().Quantities.ShouldBeEmpty();
        }

        [Theory]
        [InlineAutoMoqData("WH-A,1.2")]
        [InlineAutoMoqData("WH-A,-1")]
        public void BuildQuantities_ShouldThrowExceptionWhenUnableToParseToken(
            string warehouseInfo, [Frozen] Mock<ITokenizer> tokenizerMock, InputLineModelBuilder sut)
        {
            // arrange 
            tokenizerMock.Setup(m => m.Token).Returns(Token.WarehouseInfo);
            tokenizerMock.Setup(m => m.Value).Returns(warehouseInfo);

            // act
            var actual = Record.Exception(() => sut.BuildQuantities(tokenizerMock.Object));

            // assert
            actual.ShouldNotBeNull();
            actual.ShouldBeOfType<InputLineParsingException>();
        }

        [Theory, AutoMoqData]
        public void Build_ShouldSetIsValidToFalseWhenIdOrNameNotSet(InputLineModelBuilder sut)
        {
            // arrange 
            sut.Build();

            // act
            var actual = sut.IsValid;

            // assert
            actual.ShouldBeFalse();
        }

        [Theory, AutoMoqData]
        public void Build_ShouldSetIsValidToTrueWhenIdAndNameBothSet(Mock<ITokenizer> idTokenizerMock, string name,
                                                                     Mock<ITokenizer> nameTokenizerMock, string id,
                                                                     InputLineModelBuilder sut)
        {
            // arrange 
            idTokenizerMock.Setup(m => m.Token).Returns(Token.ProductId);
            idTokenizerMock.Setup(m => m.Value).Returns(name);
            sut.BuildId(idTokenizerMock.Object);

            nameTokenizerMock.Setup(m => m.Token).Returns(Token.ProductName);
            nameTokenizerMock.Setup(m => m.Value).Returns(id);
            sut.BuildName(nameTokenizerMock.Object);

            sut.Build();

            // act
            var actual = sut.IsValid;

            // assert
            actual.ShouldBeTrue();
        }
    }
}