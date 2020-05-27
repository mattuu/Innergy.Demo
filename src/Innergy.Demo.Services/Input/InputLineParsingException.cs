using System;

namespace Innergy.Demo.Services.Input
{
    public class InputLineParsingException : ApplicationException
    {
        public InputLineParsingException(string line)
            : base($"Unable to parse line {line}")
        {
        }

        public InputLineParsingException()
        {
        }
    }
}