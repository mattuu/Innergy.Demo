using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    public interface IInputLineParser
    {
        InputLineModel Parse(string line);
    }
}