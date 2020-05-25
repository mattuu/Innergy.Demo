using System.Collections.Generic;

namespace Innergy.Demo.Domain.Models
{
    public class InputLineModel
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public IEnumerable<InputLineQuantityModel> Quantities { get; set; }
    }
}