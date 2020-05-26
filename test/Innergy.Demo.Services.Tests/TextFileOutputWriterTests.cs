using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoFixture;
using AutoFixture.Idioms;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Tests.Infrastructure;
using Shouldly;
using Xunit;

namespace Innergy.Demo.Services.Tests
{
    public class TextFileOutputWriterTests
    {
        [Theory, AutoMoqData]
        public void Ctor_ShouldThrowExceptionOnAnyNullDependency(GuardClauseAssertion assertion)
        {
            // assert..
            assertion.Verify(typeof(TextFileOutputWriter).GetConstructors());
        }

        [Theory, AutoMoqData]
        public void Write_ShouldWriteHeaderRowForWarehouse(IFixture fixture, TextFileOutputWriter sut)
        {
            // arrange
            var models = fixture.Build<OutputGroupModel>()
                                .WithWarehouseName("ABC-123")
                                .With(m => m.Items, fixture.Build<OutputItemModel>()
                                                           .WithCount(10)
                                                           .CreateMany(1))
                                .CreateMany(1);

            var stringBuilder = new StringBuilder();
            var writer = new StringWriter(stringBuilder);

            // act
            sut.Write(writer, models);

            // assert
            var firstLine = ReadLine(stringBuilder.ToString(), 0);
            firstLine.ShouldBe("ABC-123 (total 10)");
        }

        [Theory, AutoMoqData]
        public void Write_ShouldWriteWarehouseHeadersInOrderOfTotalCountDescending(
            IFixture fixture, TextFileOutputWriter sut)
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

            var stringBuilder = new StringBuilder();
            var writer = new StringWriter(stringBuilder);

            // act
            sut.Write(writer, models);

            // assert
            var firstLine = ReadLine(stringBuilder.ToString(), 0);
            firstLine.ShouldBe("ABC-123 (total 10)");

            var fourthLine = ReadLine(stringBuilder.ToString(), 3);
            fourthLine.ShouldBe("GHI-789 (total 7)");

            var seventhLine = ReadLine(stringBuilder.ToString(), 6);
            seventhLine.ShouldBe("DEF-456 (total 3)");
        }

        [Theory, AutoMoqData]
        public void Write_ShouldWriteWarehouseHeadersInAlphabeticalOrderOfDescendingWhenTotalCountsAreTheSame(
            IFixture fixture, int count, TextFileOutputWriter sut)
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

            var stringBuilder = new StringBuilder();
            var writer = new StringWriter(stringBuilder);

            // act
            sut.Write(writer, models);

            // assert
            var firstLine = ReadLine(stringBuilder.ToString(), 0);
            firstLine.ShouldBe($"GHI-789 (total {count})");

            var fourthLine = ReadLine(stringBuilder.ToString(), 3);
            fourthLine.ShouldBe($"DEF-456 (total {count})");

            var seventhLine = ReadLine(stringBuilder.ToString(), 6);
            seventhLine.ShouldBe($"ABC-123 (total {count})");
        }

        [Theory, AutoMoqData]
        public void Write_ShouldWriteRowsForProducts(IFixture fixture, TextFileOutputWriter sut)
        {
            // arrange
            var itemModel1 = fixture.Build<OutputItemModel>()
                                    .WithCount(10)
                                    .WithId("DEF-12345")
                                    .Create();

            var itemModel2 = fixture.Build<OutputItemModel>()
                                    .WithCount(28)
                                    .WithId("ABC-44567")
                                    .Create();

            var models = fixture.Build<OutputGroupModel>()
                                .With(m => m.Items, new[] {itemModel1, itemModel2})
                                .CreateMany(1);

            var stringBuilder = new StringBuilder();
            var writer = new StringWriter(stringBuilder);

            // act
            sut.Write(writer, models);

            // assert
            var content = stringBuilder.ToString();
            var secondLine = ReadLine(content, 1);
            secondLine.ShouldBe("ABC-44567: 28");

            var thirdLine = ReadLine(content, 2);
            thirdLine.ShouldBe("DEF-12345: 10");
        }

        private static string ReadLine(string source, int index)
        {
            return source.Split("\r\n")[index];
        }
    }
}