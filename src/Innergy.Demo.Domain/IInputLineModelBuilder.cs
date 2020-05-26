using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    public interface IInputLineModelBuilder
    {
        InputLineModel Build();
        
        bool TryBuildComment(string line);
     
        bool TryBuildId(string element);
        
        bool TryBuildName(string element);
     
        void BuildQuantities(string element);
    }
}