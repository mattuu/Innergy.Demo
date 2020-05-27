using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Tests.Infrastructure;
using Shouldly;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class DefaultOutputSorterTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(DefaultOutputSorter).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void Sort_ShouldSortWarehousesByTotalCountDescending(IFixture fixture, DefaultOutputSorter sut)
        {
            // arrange
            var modelWithTotalOf10 = fixture.Build<OutputGroupModel>()
                                            .WithWarehouseName("ABC-123")
                                            .WithItems(new Dictionary<string, int> {{fixture.Create<string>(), 10}},
                                                       fixture)
                                            .CreateMany(1);

            var modelWithTotalOf3 = fixture.Build<OutputGroupModel>()
                                           .WithWarehouseName("DEF-456")
                                           .WithItems(new Dictionary<string, int> {{fixture.Create<string>(), 3}},
                                                      fixture)
                                           .CreateMany(1);

            var modelWithTotalOf7 = fixture.Build<OutputGroupModel>()
                                           .WithWarehouseName("GHI-789")
                                           .WithItems(new Dictionary<string, int> {{fixture.Create<string>(), 7}},
                                                      fixture)
                                           .CreateMany(1);

            var models = new List<OutputGroupModel>()
                        .Union(modelWithTotalOf7)
                        .Union(modelWithTotalOf10)
                        .Union(modelWithTotalOf3);

            // act
            var actual = sut.Sort(models);

            // assert
            actual.Count().ShouldBe(3);
            actual.ElementAt(0).TotalCount.ShouldBe(10);
            actual.ElementAt(0).WarehouseName.ShouldBe("ABC-123");

            actual.ElementAt(1).TotalCount.ShouldBe(7);
            actual.ElementAt(1).WarehouseName.ShouldBe("GHI-789");

            actual.ElementAt(2).TotalCount.ShouldBe(3);
            actual.ElementAt(2).WarehouseName.ShouldBe("DEF-456");
        }

        [Theory, AutoMoqData]
        public void Sort_ShouldWriteWarehouseHeadersInAlphabeticalOrderOfDescendingWhenTotalCountsAreTheSame(
            IFixture fixture, int count, DefaultOutputSorter sut)
        {
            // arrange
            var modelWithTotalOf10 = fixture.Build<OutputGroupModel>()
                                            .WithWarehouseName("ABC-123")
                                            .WithItems(new Dictionary<string, int> {{fixture.Create<string>(), count}},
                                                       fixture)
                                            .CreateMany(1);

            var modelWithTotalOf3 = fixture.Build<OutputGroupModel>()
                                           .WithWarehouseName("GHI-789")
                                           .WithItems(new Dictionary<string, int> {{fixture.Create<string>(), count}},
                                                      fixture)
                                           .CreateMany(1);

            var modelWithTotalOf7 = fixture.Build<OutputGroupModel>()
                                           .WithWarehouseName("DEF-456")
                                           .WithItems(new Dictionary<string, int> {{fixture.Create<string>(), count}},
                                                      fixture)
                                           .CreateMany(1);

            var models = new List<OutputGroupModel>()
                        .Union(modelWithTotalOf7)
                        .Union(modelWithTotalOf10)
                        .Union(modelWithTotalOf3);

            // act
            var actual = sut.Sort(models);

            // assert
            actual.Count().ShouldBe(3);
            actual.ElementAt(0).TotalCount.ShouldBe(count);
            actual.ElementAt(1).TotalCount.ShouldBe(count);
            actual.ElementAt(2).TotalCount.ShouldBe(count);

            actual.ElementAt(0).WarehouseName.ShouldBe("GHI-789");
            actual.ElementAt(1).WarehouseName.ShouldBe("DEF-456");
            actual.ElementAt(2).WarehouseName.ShouldBe("ABC-123");
        }

        [Theory, AutoMoqData]
        public void Sort_ShouldSortProductsRowsInOrderOfProductIdAscending(IFixture fixture, DefaultOutputSorter sut)
        {
            // arrange
            var itemModel1 = fixture.Build<OutputItemModel>()
                                    .WithCount(10)
                                    .WithId("ABD-12345")
                                    .Create();

            var itemModel2 = fixture.Build<OutputItemModel>()
                                    .WithCount(28)
                                    .WithId("ABC-44567")
                                    .Create();

            var models = fixture.Build<OutputGroupModel>()
                                .With(m => m.Items, new[] {itemModel1, itemModel2})
                                .CreateMany(1);

            // act
            var actual = sut.Sort(models);

            // assert
            actual.ShouldHaveSingleItem().Items.Count().ShouldBe(2);
            actual.ShouldHaveSingleItem().Items.ElementAt(0).Id.ShouldBe("ABC-44567");
            actual.ShouldHaveSingleItem().Items.ElementAt(1).Id.ShouldBe("ABD-12345");
        }
    }
}