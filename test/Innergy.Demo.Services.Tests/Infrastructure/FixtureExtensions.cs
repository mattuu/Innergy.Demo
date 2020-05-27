using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Dsl;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services.Tests.Infrastructure
{
    public static class FixtureExtensions
    {
        public static IPostprocessComposer<OutputGroupModel> WithWarehouseName(
            this IPostprocessComposer<OutputGroupModel> composer, string warehouseName)
        {
            return composer.With(m => m.WarehouseName, warehouseName);
        }

        public static IPostprocessComposer<OutputGroupModel> WithItems(
            this IPostprocessComposer<OutputGroupModel> composer, IDictionary<string, int> itemsDefinitions,
            IFixture fixture)
        {
            var items = itemsDefinitions?.Select(def => fixture.Build<OutputItemModel>()
                                                               .WithId(def.Key)
                                                               .WithCount(def.Value)
                                                               .Create());

            return items != null ? composer.With(m => m.Items, items) : composer;
        }

        public static IPostprocessComposer<OutputItemModel> WithId(
            this IPostprocessComposer<OutputItemModel> composer, string id)
        {
            return composer.With(m => m.Id, id);
        }

        public static IPostprocessComposer<OutputItemModel> WithCount(
            this IPostprocessComposer<OutputItemModel> composer, int count)
        {
            return composer.With(m => m.Count, count);
        }
    }
}