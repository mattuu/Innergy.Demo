using System.Collections.Generic;
using System.IO;
using System.Linq;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services
{
    public class TextFileOutputWriter : IOutputWriter
    {
        public void Write(TextWriter textWriter, IEnumerable<OutputGroupModel> models)
        {
            var sortedModels = models.OrderByDescending(m => m.TotalCount)
                                     .ThenByDescending(m => m.WarehouseName);

            foreach (var groupModel in sortedModels)
            {
                textWriter.WriteLine($"{groupModel.WarehouseName} (total {groupModel.TotalCount})");

                foreach (var model in groupModel.Items.OrderBy(im => im.Id))
                {
                    textWriter.WriteLine($"{model.Id}: {model.Count}");
                }

                textWriter.WriteLine();
            }
        }
    }
}