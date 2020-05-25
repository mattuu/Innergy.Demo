using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Idioms;
using Innergy.Demo.Domain.Models;
using Shouldly;
using Solex.DevTest.TestUtils;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class DataProcessorTests
    {
        private readonly Func<IFixture, string, string, IPostprocessComposer<InputLineModel>>
            _modelWithSpecificProductAndWarehouseBuilder
                = (fixture, productId, warehouseName) =>
                  {
                      return fixture.Build<InputLineModel>()
                                    .With(m => m.Id, productId)
                                    .With(m => m.Quantities, fixture
                                                            .Build<InputLineQuantityModel>()
                                                            .With(qm => qm.WarehouseName, warehouseName)
                                                            .With(qm => qm.Quantity, 1)
                                                            .CreateMany());
                  };


        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(DataProcessor).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void Process_ShouldAggregateItemsCorrectly(IFixture fixture, string productId, string warehouseName,
                                                          DataProcessor sut)
        {
            // arrange
            var models = _modelWithSpecificProductAndWarehouseBuilder(fixture, productId, warehouseName)
               .CreateMany();

            // act
            var actual = sut.Process(models);

            // assert
            actual.ShouldHaveSingleItem();
            actual.Single().WarehouseName.ShouldBe(warehouseName);
            actual.Single().TotalCount.ShouldBe(9);
            actual.Single().Items.ShouldHaveSingleItem();
            actual.Single().Items.Single().Id.ShouldBe(productId);
            actual.Single().Items.Single().Count.ShouldBe(9);
        }

        [Theory, AutoMoqData]
        public void Process_ShouldAggregateItemsCorrectly_WhenMultipleProductsExistInSingleWarehouse(
            IFixture fixture, string productAId, string productBId, string warehouseName,
            DataProcessor sut)
        {
            // arrange
            var modelA = _modelWithSpecificProductAndWarehouseBuilder(fixture, productAId, warehouseName).Create();
            var modelB = _modelWithSpecificProductAndWarehouseBuilder(fixture, productBId, warehouseName).Create();
            var models = new[] {modelA, modelB};

            // act
            var actual = sut.Process(models);

            // assert
            actual.ShouldHaveSingleItem();
            actual.Single().WarehouseName.ShouldBe(warehouseName);
            actual.Single().TotalCount.ShouldBe(6);
            actual.Single().Items.Count().ShouldBe(2);
            actual.Single().Items.ShouldContain(i => productAId.Equals(i.Id) && i.Count == 3);
            actual.Single().Items.ShouldContain(i => productBId.Equals(i.Id) && i.Count == 3);
        }
    }
}