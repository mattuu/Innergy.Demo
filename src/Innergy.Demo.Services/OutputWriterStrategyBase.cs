using System;
using System.Collections.Generic;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services
{
    public abstract class OutputWriterStrategyBase : IOutputWriterStrategy
    {
        private readonly IOutputSorter _outputSorter;
        private readonly IOutputFormatter _outputFormatter;

        protected OutputWriterStrategyBase(IOutputSorter outputSorter, IOutputFormatter outputFormatter)
        {
            _outputSorter = outputSorter ?? throw new ArgumentNullException(nameof(outputSorter));
            _outputFormatter = outputFormatter ?? throw new ArgumentNullException(nameof(outputFormatter));
        }

        public abstract void Write(IEnumerable<OutputGroupModel> models);

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