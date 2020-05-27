using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services.Input
{
    public class InputLineModelBuilder : IInputLineModelBuilder
    {
        private const string WarehouseRegex = @"WH-[A-C]{1},[-\d.]+";
        private const string WarehouseNameRegex = @"^WH-[\w]";
        private const string WarehouseQtyRegex = @"-?[\d.]+";

        private readonly Regex _warehouseNameRegex;
        private readonly Regex _warehouseQtyRegex;
        private readonly Regex _warehouseRegex;
        private string _id;
        private string _name;
        private ICollection<InputLineQuantityModel> _quantities = new List<InputLineQuantityModel>();

        public InputLineModelBuilder()
        {
            _warehouseRegex = new Regex(WarehouseRegex);
            _warehouseNameRegex = new Regex(WarehouseNameRegex);
            _warehouseRegex = new Regex(WarehouseRegex);
            _warehouseQtyRegex = new Regex(WarehouseQtyRegex);
        }

        public bool IsComment { get; private set; }

        public bool IsValid { get; private set; }

        public InputLineModel Build()
        {
            if (IsComment)
            {
                return null;
            }

            IsValid = !string.IsNullOrEmpty(_id) && !string.IsNullOrEmpty(_name);

            var model = new InputLineModel
                        {
                            Id = _id,
                            Name = _name,
                            Quantities = _quantities
                        };

            _id = null;
            _name = null;
            _quantities = new List<InputLineQuantityModel>();

            return model;
        }

        public void BuildComment(ITokenizer tokenizer)
        {
            IsComment = tokenizer.Token == Token.Comment;
        }

        public void BuildId(ITokenizer tokenizer)
        {
            if (tokenizer.Token == Token.ProductId)
            {
                _id = tokenizer.Value;
            }
        }

        public void BuildName(ITokenizer tokenizer)
        {
            if (tokenizer.Token == Token.ProductName)
            {
                _name = tokenizer.Value;
            }
        }

        public void BuildQuantities(ITokenizer tokenizer)
        {
            if (tokenizer.Token != Token.WarehouseInfo)
            {
                return;
            }

            if (_warehouseRegex.IsMatch(tokenizer.Value))
            {
                var warehouseMatches = _warehouseRegex.Matches(tokenizer.Value);
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
                            throw new InputLineParsingException($"Quantity is not integer: {warehouseQtyMatch.Value}");
                        }
                    }
                    else
                    {
                        throw new
                            InputLineParsingException($"Warehouse name cannot be inferred from input string: {warehouseNameMatch.Value}");
                    }
                }

                _quantities = quantityList;
            }
        }
    }
}