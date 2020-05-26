using System;
using System.Linq;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services
{
    public class InputLineParser : IInputLineParser
    {
        public const string ElementDelimiter = ";";
        private readonly IInputLineModelBuilder _builder;

        public InputLineParser(IInputLineModelBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public InputLineModel Parse(string line)
        {
            // ignore comment line
            if (_builder.TryBuildComment(line))
            {
                return null;
            }

            var elements = line.Split(ElementDelimiter.ToCharArray()).AsEnumerable();

            var idElement = elements.Select(e => _builder.TryBuildId(e) ? e : null);
            if (idElement != null)
            {
                elements = elements.Except(idElement);

                var nameElement = elements.Select(e => _builder.TryBuildName(e) ? e : null);
                elements = elements.Except(nameElement);

                foreach (var element in elements)
                {
                    _builder.BuildQuantities(element);
                }
            }


            //foreach (var element in elements)
            //{
            //    if (!_builder.TryBuildId(element))
            //    {
            //        _builder.TryBuildName(element);
            //    }

            //    _builder.BuildQuantities(element);
            //}

            return _builder.Build();
        }
    }
}