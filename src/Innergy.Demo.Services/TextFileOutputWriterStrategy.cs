using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services
{
    public class TextFileOutputWriterStrategy : OutputWriterStrategyBase
    {
        private readonly string _outputFilePath;

        public TextFileOutputWriterStrategy(string outputFilePath, IOutputSorter outputSorter, IOutputFormatter outputFormatter)
            : base(outputSorter, outputFormatter)
        {
            _outputFilePath = outputFilePath ?? throw new ArgumentNullException(nameof(outputFilePath));
        }

        public override void Write(IEnumerable<OutputGroupModel> models)
        {
            using (var textWriter = new FileInfo(_outputFilePath).CreateText())
            {
                foreach (var groupModel in base.SortItems(models))
                {
                    textWriter.WriteLine(FormatGroupHeader(groupModel));

                    foreach (var model in groupModel.Items)
                    {
                        textWriter.WriteLine(FormatLine(model));
                    }

                    textWriter.WriteLine();
                }
            }
        }
    }
}