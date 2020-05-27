using System;
using System.Collections.Generic;
using System.IO;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    [Obsolete]
    public interface IInputReader
    {
        IEnumerable<InputLineModel> Parse(StreamReader streamReader);
    }
}