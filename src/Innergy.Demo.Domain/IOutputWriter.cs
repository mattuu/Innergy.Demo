using System.Collections.Generic;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    public interface IOutputWriter
    {
        void Write(IEnumerable<OutputGroupModel> models);
        
        string FormatGroupHeader(OutputGroupModel model);
        
        string FormatLine(OutputItemModel model);
    }
}