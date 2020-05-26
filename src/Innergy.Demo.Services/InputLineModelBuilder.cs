using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services
{
    public class InputLineModelBuilder : IInputLineModelBuilder
    {
        public const string CommentPrefix = "#";
        public const string ElementDelimiter = ";";

        private const string IdRegex = @"^[A-Z0-9]{2,6}-\w{2,}";
        private const string WarehouseRegex = @"WH-[A-C]{1},[-\d.]+";
        private const string WarehouseNameRegex = @"^WH-[\w]";
        private readonly Regex _idRegex;
        private readonly InputLineModel _inputLineModel;
        private readonly Regex _warehouseNameRegex;
        private readonly Regex _warehouseQtyRegex;
        private readonly Regex _warehouseRegex;
        private string _id;
        private string _name;
        private IEnumerable<InputLineQuantityModel> _quantities;
        private string _line;

        public InputLineModelBuilder()
        {
            _inputLineModel = new InputLineModel();
            _idRegex = new Regex(IdRegex);
            _warehouseRegex = new Regex(WarehouseRegex);
            _warehouseQtyRegex = new Regex(@"-?[\d.]+");
            _warehouseNameRegex = new Regex(WarehouseNameRegex);
        }

        public InputLineModel Build()
        {
            if (string.IsNullOrEmpty(_id) || string.IsNullOrEmpty(_name))
            {
                throw new InputLineParsingException(_line);
            }

            var model = new InputLineModel
                        {
                            Id = _id,
                            Name = _name,
                            Quantities = _quantities
                        };

            _id = null;
            _name = null;
            _quantities = null;

            return model;
        }

        public bool TryBuildId(string line, string element)
        {
            _line = line;
          
            var idRegexMatch = _idRegex.Match(element);
            if (!idRegexMatch.Success)
            {
                return false;
            }

            _id = idRegexMatch.Value;
            return true;
        }

        public bool TryBuildName(string element)
        {
            if (!string.IsNullOrEmpty(_inputLineModel.Name) || string.IsNullOrEmpty(element))
            {
                return false;
            }

            _name = element;
            return true;
        }

        public void BuildQuantities(string element)
        {
            if (_warehouseRegex.IsMatch(element))
            {
                var warehouseMatches = _warehouseRegex.Matches(element);
                var quantityList = new Collection<InputLineQuantityModel>();
                foreach (Match match in warehouseMatches)
                {
                    var warehouseNameMatch = _warehouseNameRegex.Match(match.Value);
                    var warehouseQtyMatch = _warehouseQtyRegex.Match(match.Value);

                    if (warehouseNameMatch.Success && warehouseQtyMatch.Success)
                    {
                        var qtyModel = new InputLineQuantityModel
                                       {
                                           WarehouseName = warehouseNameMatch.Value
                                       };

                        if (int.TryParse(warehouseQtyMatch.Value, out var qty) && qty > 0)
                        {
                            qtyModel.Quantity = qty;
                            quantityList.Add(qtyModel);
                        }
                        else
                        {
                            throw new InputLineParsingException(_line);
                        }
                    }
                    else
                    {
                        throw new InputLineParsingException(_line);
                    }
                }

                _quantities = quantityList;
            }
        }
    }
}