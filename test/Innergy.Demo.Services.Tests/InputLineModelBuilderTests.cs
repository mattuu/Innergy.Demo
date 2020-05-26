using AutoFixture.Idioms;
using Innergy.Demo.Services.Tests.Infrastructure;
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
        public void TryBuildComment_ShouldReturnTrueWhenLineStartsWithCommentPrefix(
            string line, InputLineModelBuilder sut)
        {
            // arrange 
            var commentLine = $"{InputLineModelBuilder.CommentPrefix}{line}";

            // act
            var actual = sut.TryBuildComment(commentLine);

            // assert
            actual.ShouldBeTrue();
        }

        [Theory, AutoMoqData]
        public void TryBuildComment_ShouldReturnFalseForNonCommentLine(string line, InputLineModelBuilder sut)
        {
            // act
            var actual = sut.TryBuildComment(line);

            // assert
            actual.ShouldBeFalse();
        }

        [Theory]
        [InlineAutoMoqData("ABC-123")]
        [InlineAutoMoqData("3M-Cherry-10mm")]
        [InlineAutoMoqData("COM-100001")]
        [InlineAutoMoqData("COM-123906c")]
        [InlineAutoMoqData("COM-123908")]
        [InlineAutoMoqData("COM-124047")]
        public void TryBuildId_ShouldReturnTrueIfValueConformsPattern(string line, InputLineModelBuilder sut)
        {
            // act
            var actual = sut.TryBuildId(line);

            // assert
            actual.ShouldBeTrue();
        }

        [Theory, AutoMoqData]
        public void TryBuildId_ShouldReturnFalseIfValueDoesNotConformPattern(string line, InputLineModelBuilder sut)
        {
            // act
            var actual = sut.TryBuildId(line);

            // assert
            actual.ShouldBeFalse();
        }

        [Theory, AutoMoqData]
        public void TryBuildName_ShouldReturnTrueIfValueIsNotEmpty(string line, InputLineModelBuilder sut)
        {
            // act
            var actual = sut.TryBuildName(line);

            // assert
            actual.ShouldBeTrue();
        }

        [Theory, AutoMoqData]
        public void TryBuildName_ShouldReturnFalseIfValueIsEmpty(InputLineModelBuilder sut)
        {
            // act
            var actual = sut.TryBuildName(null);

            // assert
            actual.ShouldBeFalse();
        }


        // TODO test BuildQuantities()

        [Theory]
        [InlineAutoMoqData("ABC-123", "this is a name", "WH-A", 3)]
        public void Build_ShouldReturnCorrectObject(string id, string name, string warehouse, int count,
                                                    InputLineModelBuilder sut)
        {
            // arrange
            var line = $"{id};{name};{warehouse},{count}";
            sut.TryBuildComment(line);
            sut.TryBuildId(id);
            sut.TryBuildName(name);
            sut.BuildQuantities($"{warehouse},{count}");

            // act
            var actual = sut.Build();

            // assert
            actual.ShouldNotBeNull();
            actual.Id.ShouldBe(id);
            actual.Name.ShouldBe(name);
            actual.Quantities.ShouldContain(q => warehouse.Equals(q.WarehouseName) && q.Quantity == count);
        }

        [Theory]
        [InlineAutoMoqData("this is a name", "ABC-123", "WH-A", 3)]
        public void Build_ShouldReturnCorrectObjectWhenIdAndNameAreOtherWayRound(string id, string name, string warehouse, int count,
                                                    InputLineModelBuilder sut)
        {
            // arrange
            var line = $"{id};{name};{warehouse},{count}";
            sut.TryBuildComment(line);
            sut.TryBuildId(id);
            sut.TryBuildName(name);
            sut.BuildQuantities($"{warehouse},{count}");

            // act
            var actual = sut.Build();

            // assert
            actual.ShouldNotBeNull();
            actual.Id.ShouldBe(id);
            actual.Name.ShouldBe(name);
            actual.Quantities.ShouldContain(q => warehouse.Equals(q.WarehouseName) && q.Quantity == count);
        }

    }
}