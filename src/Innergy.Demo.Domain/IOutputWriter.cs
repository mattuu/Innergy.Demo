using System.Collections.Generic;
using System.IO;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    public interface IOutputWriter
    {
        void Write(TextWriter textWriter, IEnumerable<OutputGroupModel> models);
    }
}