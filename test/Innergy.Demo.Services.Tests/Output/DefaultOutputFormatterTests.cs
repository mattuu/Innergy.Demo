using AutoFixture;
using AutoFixture.Idioms;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Output;
using Innergy.Demo.Services.Tests.Infrastructure;
using Shouldly;
using Xunit;

namespace Innergy.Demo.Services.Tests.Output
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
            actual.ShouldStartWith("WarehouseName (total 5)");
        }

        [Theory, AutoMoqData]
        public void FormatWarehouse_ShouldWriteOutProductsOnSeparateLines(IFixture fixture, DefaultOutputFormatter sut)
        {
            // arrange
            var model = fixture.Build<OutputGroupModel>()
                               .WithWarehouseName("WarehouseName")
                               .With(m => m.Items, 
                                     new []
                                     {
                                         fixture.Build<OutputItemModel>()
                                                .WithId("ProductA")
                                                .WithCount(1)
                                                .Create(),
                                         fixture.Build<OutputItemModel>()
                                                .WithId("ProductB")
                                                .WithCount(1)
                                                .Create()
                                     })
                               .Create();

            // act
            var actual = sut.FormatWarehouse(model);

            // assert
            actual.ShouldBe(@"WarehouseName (total 2)
ProductA: 1
ProductB: 1
");
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
            actual.ShouldBe("ABC-123: 4");
        }
    }
}