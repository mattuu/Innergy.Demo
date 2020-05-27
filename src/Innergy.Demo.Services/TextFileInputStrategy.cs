using System.Collections.Generic;
using System.IO;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Innergy.Demo.Services
{
    public class TextFileInputStrategy : InputStrategyBase, IInputStrategy
    {
        public TextFileInputStrategy(ILogger<InputStrategyBase> logger, IInputLineParser inputLineParser) :
            base(logger, inputLineParser)
        {
        }

        public IEnumerable<InputLineModel> Load(string source)
        {
            using (var streamReader = File.OpenText(source))
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