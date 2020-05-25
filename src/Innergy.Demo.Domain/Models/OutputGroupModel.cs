using System.Collections.Generic;
using System.Linq;

namespace Innergy.Demo.Domain.Models
{
    public class OutputGroupModel
    {
        public string WarehouseName { get; set; }

        public int TotalCount => Items?.Sum(i => i.Count) ?? 0;

        public IEnumerable<OutputItemModel> Items { get; set; }
    }
}