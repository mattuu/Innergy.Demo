using AutoFixture;
using AutoFixture.Idioms;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Tests.Infrastructure;
using Shouldly;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class DefaultOutputFormatterTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(DefaultOutputFormatter).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void FormatWarehouse_ShouldReturnCorrectResult(IFixture fixture, DefaultOutputFormatter sut)
        {
            // arrange
            var model = fixture.Build<OutputGroupModel>()
                               .WithWarehouseName("WarehouseName")
                               .With(m => m.Items, fixture.Build<OutputItemModel>().WithCount(1).CreateMany(5))
                               .Create();

            // act
            var actual = sut.FormatWarehouse(model);

            // assert
            actual.ShouldBe("WarehouseName (total 5)");
        }

        [Theory, AutoMoqData]
        public void FormatProduct_ShouldReturnCorrectResult(IFixture fixture, DefaultOutputFormatter sut)
        {
            // arrange
            var model = fixture.Build<OutputItemModel>()
                               .WithId("ABC-123")
                               .WithCount(4)
                               .Create();

            // act
            var actual = sut.FormatProduct(model);

            // assert
            actual.ShouldBe("ABC-123: 4)");
        }
    }
}