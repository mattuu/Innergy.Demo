using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Innergy.Demo.Services
{
    public class TextFileInputReader : IInputReader
    {
        private readonly IInputLineParser _inputLineParser;
        private readonly ILogger _logger;

        public TextFileInputReader(IInputLineParser inputLineParser, ILogger<TextFileInputReader> logger)
        {
            _inputLineParser = inputLineParser ?? throw new ArgumentNullException(nameof(inputLineParser));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<InputLineModel> Parse(StreamReader streamReader)
        {
            var list = new Collection<InputLineModel>();
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();

                try
                {
                    var item = _inputLineParser.Parse(line);
                    if (item != null)
                    {
                        list.Add(item);
                    }
                }
                catch (InputLineParsingException e)
                {
                    _logger.LogWarning(e, e.Message);
                }
            }

            return list;
        }
    }
}