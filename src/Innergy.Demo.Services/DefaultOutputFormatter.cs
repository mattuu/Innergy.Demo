using System.Text;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services
{
    public class DefaultOutputFormatter : IOutputFormatter
    {
        public string FormatWarehouse(OutputGroupModel model)
        {
            var outputStringBuilder = new StringBuilder();
            outputStringBuilder.AppendLine($"{model.WarehouseName} (total {model.TotalCount})");
            foreach (var outputItemModel in model.Items)
            {
                outputStringBuilder.AppendLine(FormatProduct(outputItemModel));
            }

            return outputStringBuilder.ToString();
        }

        public string FormatProduct(OutputItemModel model)
        {
            return $"{model.Id}: {model.Count}";
        }
    }
}