using System.Collections.Generic;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Domain
{
    public interface IOutputSorter
    {
        IEnumerable<OutputGroupModel> Sort(IEnumerable<OutputGroupModel> source);
    }
}