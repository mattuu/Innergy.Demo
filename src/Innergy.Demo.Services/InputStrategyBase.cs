using System;
using System.Collections.Generic;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Innergy.Demo.Services
{
    public abstract class InputStrategyBase
    {
        private readonly IInputLineParser _inputLineParser;
        private readonly ILogger _logger;
        private readonly ICollection<InputLineModel> _models;

        protected InputStrategyBase(ILogger<InputStrategyBase> logger, IInputLineParser inputLineParser)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _inputLineParser = inputLineParser ?? throw new ArgumentNullException(nameof(inputLineParser));
            _models = new List<InputLineModel>();
        }

        public void ParseLine(string line)
        {
            try
            {
                var item = _inputLineParser.Parse(line);
                if (item != null)
                {
                    _models.Add(item);
                }
            }
            catch (InputLineParsingException e)
            {
                // TODO introduce line parsing error observer
                _logger.LogWarning(e, e.Message);
            }
        }

        public IEnumerable<InputLineModel> GetModels()
        {
            return _models;
        }
    }
}