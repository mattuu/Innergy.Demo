using System.Collections.Generic;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    public interface IDataProcessor
    {
        IEnumerable<OutputGroupModel> Process(IEnumerable<InputLineModel> models);
    }
}