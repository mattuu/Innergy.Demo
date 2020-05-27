using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    public interface IInputLineModelBuilder
    {
        InputLineModel Build();

        void BuildQuantities(ITokenizer tokenizer);
        
        void BuildComment(ITokenizer tokenizer);
        
        void BuildId(ITokenizer tokenizer);
        
        void BuildName(ITokenizer tokenizer);
    }
}