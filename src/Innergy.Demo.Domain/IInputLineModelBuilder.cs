using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    public interface IInputLineModelBuilder
    {
        InputLineModel Build();
        bool TryBuildId(string line, string element);
        bool TryBuildName(string element);
        void BuildQuantities(string element);
    }
}