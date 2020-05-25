using System;
using System.Collections.Generic;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Innergy.Demo.Services
{
    public class DataProcessor : IDataProcessor
    {
        private readonly ILogger _logger;

        public DataProcessor(ILogger<DataProcessor> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<OutputGroupModel> Process(IEnumerable<InputLineModel> models)
        {
            throw new NotImplementedException();
        }
    }
}