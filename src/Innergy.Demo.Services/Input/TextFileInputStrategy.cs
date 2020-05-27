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
        private readonly string _filePath;

        public TextFileInputStrategy(ILogger<InputStrategyBase> logger, IInputLineParser inputLineParser,
                                     string filePath)
            : base(logger, inputLineParser)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public IEnumerable<InputLineModel> Load()
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
    }
}