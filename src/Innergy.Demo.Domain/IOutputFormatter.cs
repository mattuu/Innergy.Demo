using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    public interface IOutputFormatter
    {
        string FormatWarehouse(OutputGroupModel model);

        string FormatProduct(OutputItemModel model);
    }
}