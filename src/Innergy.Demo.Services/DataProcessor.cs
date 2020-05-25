using System.Collections.Generic;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services
{
    public class DataProcessor : IDataProcessor

    {
        public IEnumerable<OutputGroupModel> Process(IEnumerable<InputLineModel> models)
        {
            throw new System.NotImplementedException();
        }
    }
}