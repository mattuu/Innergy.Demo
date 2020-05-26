using System;
using System.Collections.Generic;
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
        private readonly Func<IFixture, string, IDictionary<string, int>, IPostprocessComposer<InputLineModel>>
            _modelBuilder
                = (fixture, productId, warehouseDef) =>
                  {
                      var postprocessComposer = fixture.Build<InputLineModel>()
                                                       .With(m => m.Id, productId);

                      if (warehouseDef == null)
                      {
                          postprocessComposer.Without(m => m.Quantities);
                      }
                      else
                      {
                          var quantityModels = warehouseDef.Select(def =>
                                                                   {
                                                                       return fixture
                                                                             .Build<InputLineQuantityModel>()
                                                                             .With(m => m.WarehouseName, def.Key)
                                                                             .With(m => m.Quantity, def.Value)
                                                                             .Create();
                                                                   }
                                                                  ).ToList();

                          postprocessComposer = postprocessComposer.With(m => m.Quantities, quantityModels);
                      }

                      return postprocessComposer;
                  };

        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(DataProcessor).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void Process_ShouldAggregateItemsCorrectly_ForSingleProductInSingleWarehouse(IFixture fixture,
                                                                                            string productId,
                                                                                            int productQty,
                                                                                            string warehouseName,
                                                                                            DataProcessor sut)
        {
            // arrange
            var model = _modelBuilder(fixture, productId, new Dictionary<string, int> {{warehouseName, productQty}})
               .Create();

            // act
            var actual = sut.Process(new[] {model});

            // assert
            actual.ShouldHaveSingleItem().WarehouseName.ShouldBe(warehouseName);
            actual.ShouldHaveSingleItem().TotalCount.ShouldBe(productQty);
            actual.ShouldHaveSingleItem().Items.ShouldHaveSingleItem().Id.ShouldBe(productId);
            actual.ShouldHaveSingleItem().Items.ShouldHaveSingleItem().Count.ShouldBe(productQty);
        }

        [Theory, AutoMoqData]
        public void Process_ShouldAggregateItemsCorrectly_ForMultipleProductsInSingleWarehouse(IFixture fixture,
                                                                                               string productAId,
                                                                                               string productBId,
                                                                                               string
                                                                                                   warehouseName,
                                                                                               int product1Qty,
                                                                                               int product2Qty,
                                                                                               DataProcessor sut)
        {
            // arrange
            var modelA = _modelBuilder(fixture, productAId, new Dictionary<string, int> {{warehouseName, product1Qty}})
               .Create();
            var modelB = _modelBuilder(fixture, productBId, new Dictionary<string, int> {{warehouseName, product2Qty}})
               .Create();
            var models = new[] {modelA, modelB};

            // act
            var actual = sut.Process(models);

            // assert
            actual.ShouldHaveSingleItem().WarehouseName.ShouldBe(warehouseName);
            actual.ShouldHaveSingleItem().TotalCount.ShouldBe(product1Qty + product2Qty);
            actual.ShouldHaveSingleItem().Items.Count().ShouldBe(2);
            actual.ShouldHaveSingleItem().Items.ShouldContain(i => productAId.Equals(i.Id) && i.Count == product1Qty);
            actual.ShouldHaveSingleItem().Items.ShouldContain(i => productBId.Equals(i.Id) && i.Count == product2Qty);
        }

        [Theory, AutoMoqData]
        public void Process_ShouldAggregateItemsCorrectly_ForSingleProductInMultipleWarehouses(IFixture fixture,
                                                                                               string productId,
                                                                                               int productQty1,
                                                                                               int productQty2,
                                                                                               string wh1Name,
                                                                                               string wh2Name,
                                                                                               DataProcessor sut)
        {
            // arrange
            var model = _modelBuilder(fixture, productId,
                                      new Dictionary<string, int> {{wh1Name, productQty1}, {wh2Name, productQty2}})
               .Create();

            // act
            var actual = sut.Process(new[] {model});

            // assert
            actual.ShouldNotBeNull();
            actual.Count().ShouldBe(2);
            actual.ShouldContain(g => wh1Name.Equals(g.WarehouseName) &&
                                      g.TotalCount == productQty1 &&
                                      g.Items.Count() == 1 &&
                                      g.Items.Single().Count == productQty1);

            actual.ShouldContain(g => wh2Name.Equals(g.WarehouseName) &&
                                      g.TotalCount == productQty2 &&
                                      g.Items.Count() == 1 &&
                                      g.Items.Single().Count == productQty2);
        }

        [Theory, AutoMoqData]
        public void Process_ShouldAggregateItemsCorrectly_ForMultipleProductsInMultipleWarehouses(IFixture fixture,
                                                                                                  string product1Id,
                                                                                                  string product2Id,
                                                                                                  string wh1Name,
                                                                                                  string wh2Name,
                                                                                                  string wh3Name,
                                                                                                  DataProcessor sut)
        {
            // arrange
            var modelA = _modelBuilder(fixture, product1Id,
                                       new Dictionary<string, int> {{wh1Name, 2}, {wh2Name, 3}, {wh3Name, 1}})
               .Create();

            var modelB = _modelBuilder(fixture, product2Id, new Dictionary<string, int> {{wh1Name, 5}, {wh2Name, 6}})
               .Create();

            var models = new[] {modelA, modelB};

            // act
            var actual = sut.Process(models);

            // assert
            actual.ShouldNotBeNull();
            actual.Count().ShouldBe(3);
            actual.ShouldContain(g => wh1Name.Equals(g.WarehouseName) &&
                                      g.TotalCount == 7 &&
                                      g.Items.Count() == 2 &&
                                      g.Items.SingleOrDefault(i => product1Id.Equals(i.Id) && i.Count == 2) != null &&
                                      g.Items.SingleOrDefault(i => product2Id.Equals(i.Id) && i.Count == 5) != null);

            actual.ShouldContain(g => wh2Name.Equals(g.WarehouseName) &&
                                      g.TotalCount == 9 &&
                                      g.Items.Count() == 2 &&
                                      g.Items.SingleOrDefault(i => product1Id.Equals(i.Id) && i.Count == 3) != null &&
                                      g.Items.SingleOrDefault(i => product2Id.Equals(i.Id) && i.Count == 6) != null);

            actual.ShouldContain(g => wh3Name.Equals(g.WarehouseName) &&
                                      g.TotalCount == 1 &&
                                      g.Items.Count() == 1 &&
                                      product1Id.Equals(g.Items.ShouldHaveSingleItem().Id) &&
                                      g.Items.ShouldHaveSingleItem().Count == 1);
        }
    }
}