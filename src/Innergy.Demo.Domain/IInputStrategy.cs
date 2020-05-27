using System.Collections.Generic;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    public interface IInputStrategy
    {
        IEnumerable<InputLineModel> Load(string source);
    }
}