using System;
using System.Collections.Generic;
using System.Text;

namespace Innergy.Demo.Domain
{
    public interface ITokenizer
    {
        void NextToken();
        
        Token Token { get; }
        
        string Value { get; }
    }
}
