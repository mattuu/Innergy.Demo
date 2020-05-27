using System.Collections.Generic;
using System.Linq;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services.Output
{
    public class DefaultOutputSorter : IOutputSorter
    {
        public IEnumerable<OutputGroupModel> Sort(IEnumerable<OutputGroupModel> source)
        {
            var sortedModels = source.OrderByDescending(m => m.TotalCount)
                                     .ThenByDescending(m => m.WarehouseName);

            foreach (var outputGroupModel in sortedModels)
            {
                yield return new OutputGroupModel
                             {
                                 WarehouseName = outputGroupModel.WarehouseName,
                                 Items = outputGroupModel.Items.OrderBy(i => i.Id)
                             };
            }
        }
        //    var sortedModels = source.OrderByDescending(m => m.TotalCount)
        //                             .ThenByDescending(m => m.WarehouseName);

        //    foreach (var groupModel in sortedModels)
        //    {
        //        textWriter.WriteLine($"{groupModel.WarehouseName} (total {groupModel.TotalCount})");

        //        foreach (var model in groupModel.Items.OrderBy(im => im.Id))
        //        {
        //            textWriter.WriteLine($"{model.Id}: {model.Count}");
        //        }

        //        textWriter.WriteLine();
        //    }
        //}
    }
}