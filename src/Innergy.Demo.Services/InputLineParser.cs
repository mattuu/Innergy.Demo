using System;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services
{
    public class InputLineParser : IInputLineParser
    {
        public InputLineModel Parse(string line)
        {
            // TODO
            // ignore comment line
            // handle malformed lines
            // use regex to capture item ID: [A-Z0-9]{2,6}-\w+

            throw new NotImplementedException();
        }
    }
}