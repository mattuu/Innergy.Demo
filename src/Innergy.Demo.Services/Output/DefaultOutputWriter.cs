using System;
using System.Collections.Generic;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services.Output
{
    public class DefaultOutputWriter : IOutputWriter
    {
        private readonly IOutputSorter _outputSorter;
        private readonly IOutputFormatter _outputFormatter;
        private readonly IWriterStrategy _writerStrategy;

        public DefaultOutputWriter(IOutputSorter outputSorter, IOutputFormatter outputFormatter, IWriterStrategy writerStrategy)
        {
            _outputSorter = outputSorter ?? throw new ArgumentNullException(nameof(outputSorter));
            _outputFormatter = outputFormatter ?? throw new ArgumentNullException(nameof(outputFormatter));
            _writerStrategy = writerStrategy ?? throw new ArgumentNullException(nameof(writerStrategy));
        }

        public virtual void Write(IEnumerable<OutputGroupModel> models)
        {
            foreach (var groupModel in SortItems(models))
            {
                _writerStrategy.WriteLine(FormatGroupHeader(groupModel));
            }
        }

        public IEnumerable<OutputGroupModel> SortItems(IEnumerable<OutputGroupModel> source)
        {
            return _outputSorter.Sort(source);
        }

        public string FormatGroupHeader(OutputGroupModel model)
        {
            return _outputFormatter.FormatWarehouse(model);
        }

        public string FormatLine(OutputItemModel model)
        {
            return _outputFormatter.FormatProduct(model);
        }
    }
}