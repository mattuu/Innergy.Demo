using System;

namespace Innergy.Demo.Services
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