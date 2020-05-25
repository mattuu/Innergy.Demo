using System.Collections.Generic;
using System.IO;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    public interface IInputReader
    {
        IEnumerable<InputLineModel> Parse(Stream stream);
    }
}