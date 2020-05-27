using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services
{
    public class DefaultOutputFormatter : IOutputFormatter
    {
        public string FormatWarehouse(OutputGroupModel model)
        {
            return $"{model.WarehouseName} (total {model.TotalCount})";
        }

        public string FormatProduct(OutputItemModel model)
        {
            return $"{model.Id}: {model.Count}";
        }
    }
}