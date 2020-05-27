using System;
using System.Collections.Generic;
using System.IO;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Innergy.Demo.Services.Input
{
    public class TextFileInputStrategy : InputStrategyBase, IInputStrategy
    {
        private readonly ILogger<InputStrategyBase> _logger;
        private readonly string _filePath;

        public TextFileInputStrategy(ILogger<InputStrategyBase> logger, IInputLineParser inputLineParser,
                                     string filePath)
            : base(logger, inputLineParser)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public IEnumerable<InputLineModel> Load()
        {
            try
            {
                using (var streamReader = File.OpenText(_filePath))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        ParseLine(line);
                    }

                    return GetModels();
                }
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}