﻿using System;
using System.Collections.Generic;
using System.Linq;
using Innergy.Demo.Domain;
using Innergy.Demo.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Innergy.Demo.Services
{
    public class DataProcessor : IDataProcessor
    {
        private readonly ILogger _logger;

        public DataProcessor(ILogger<DataProcessor> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<OutputGroupModel> Process(IEnumerable<InputLineModel> models)
        {
            if (models == null)
            {
                _logger.LogWarning("Empty collection passed into data processor.");
                return new OutputGroupModel[0];
            }

            var deflated = models.SelectMany(m => m.Quantities,
                                             (lm, qm) =>
                                                 new
                                                 {
                                                     ProductId = lm.Id,
                                                     ProductName = lm.Name,
                                                     qm.WarehouseName,
                                                     qm.Quantity
                                                 })
                                 .GroupBy(x => x.WarehouseName)
                                 .Select(g => new OutputGroupModel
                                              {
                                                  WarehouseName = g.Key,
                                                  Items = g.GroupBy(i => i.ProductId)
                                                           .Select(ig => new OutputItemModel
                                                                         {
                                                                             Name = ig.FirstOrDefault()?.ProductName,
                                                                             Id = ig.Key,
                                                                             Count = ig.Sum(_ => _.Quantity)
                                                                         })
                                              });

            return deflated;
        }
    }
}