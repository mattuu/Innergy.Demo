using System;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services
{
    public class InputLineParser : IInputLineParser
    {
        public const string CommentPrefix = "#";
        public const string ElementDelimiter = ";";
        private readonly IInputLineModelBuilder _builder;

        public InputLineParser(IInputLineModelBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public InputLineModel Parse(string line)
        {
            // ignore comment line
            if (line.StartsWith(CommentPrefix))
            {
                return null;
            }

            var elements = line.Split(ElementDelimiter.ToCharArray());

            foreach (var element in elements)
            {

                if (!_builder.TryBuildId(line, element))
                {
                    _builder.TryBuildName(element);
                }

                _builder.BuildQuantities(element);
            }

            return _builder.Build();
        }
    }
}