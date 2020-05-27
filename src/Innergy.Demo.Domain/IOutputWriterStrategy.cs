using System.Collections.Generic;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    public interface IOutputWriterStrategy
    {
        void Write(IEnumerable<OutputGroupModel> models);
        
        string FormatGroupHeader(OutputGroupModel model);
        
        string FormatLine(OutputItemModel model);
    }
}