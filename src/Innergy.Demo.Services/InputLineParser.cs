using System;
using System.IO;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Innergy.Demo.Services.Parsing;

namespace Innergy.Demo.Services
{
    public class InputLineParser : IInputLineParser
    {
        private readonly IInputLineModelBuilder _builder;

        public InputLineParser(IInputLineModelBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public InputLineModel Parse(string line)
        {
            using (var stringReader = new StringReader(line))
            {
                var tokenizer = new Tokenizer(stringReader);

                while (tokenizer.Token != Token.EOL)
                {
                    _builder.BuildComment(tokenizer);
                    _builder.BuildId(tokenizer);
                    _builder.BuildName(tokenizer);
                    _builder.BuildQuantities(tokenizer);
                    tokenizer.NextToken();
                }
            }

            return _builder.Build();
        }
    }
}