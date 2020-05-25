using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;

namespace Innergy.Demo.Services
{
    public class InputLineParser : IInputLineParser
    {
        public const string CommentPrefix = "#";
        public const string ElementDelimiter = ";";

        private const string IdRegex = @"^[A-Z0-9]{2,6}-\w{2,}";
        private const string WarehouseRegex = @"WH-[A-C]{1},[-\d.]+";
        private const string WarehouseNameRegex = @"^WH-[\w]";

        public InputLineModel Parse(string line)
        {
            // ignore comment line
            if (line.StartsWith(CommentPrefix))
            {
                return null;
            }

            var elements = line.Split(ElementDelimiter.ToCharArray());

            var model = new InputLineModel();

            var idRegex = new Regex(IdRegex);
            var warehouseRegex = new Regex(WarehouseRegex);
            var warehouseQtyRegex = new Regex(@"-?[\d.]+");
            var warehouseNameRegex = new Regex(WarehouseNameRegex);

            foreach (var element in elements)
            {
                // use regex to capture item ID: [A-Z0-9]{2,6}-\w+
                var idRegexMatch = idRegex.Match(element);
                if (idRegexMatch.Success)
                {
                    model.Id = idRegexMatch.Value;
                }
                else
                {
                    if (warehouseRegex.IsMatch(element))
                    {
                        var warehouseMatches = warehouseRegex.Matches(element);
                        var quantityList = new Collection<InputLineQuantityModel>();
                        foreach (Match match in warehouseMatches)
                        {
                            var warehouseNameMatch = warehouseNameRegex.Match(match.Value);
                            var warehouseQtyMatch = warehouseQtyRegex.Match(match.Value);

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
                                    throw new InputLineParsingException(line);
                                }
                            }
                            else
                            {
                                throw new InputLineParsingException(line);
                            }
                        }

                        model.Quantities = quantityList;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(element))
                        {
                            model.Name = element;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.Name))
            {
                throw new InputLineParsingException(line);
            }

            return model;
        }
    }
}