using GroceryScanner.Core.Services;
using GroceryScanner.Core.Services.Products.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;

namespace GroceryScanner.Services
{
    public class PointOfSaleTerminalService : IPointOfSaleTerminalService
    {
        public void SetPricing()
        {
            var productList = new List<Product>
            {
                new Product
                {
                    Code = "A",
                    UnitPrice = 1.25M,
                    MultiBuyOption = new MultiBuyOption
                    {
                        MultiBuyUnitCount = 3,
                        MultiBuyPriceTotal = 3.00M
                    }
                },
                new Product
                {
                    Code = "B",
                    UnitPrice = 4.25M
                },
                new Product
                {
                    Code = "C",
                    UnitPrice = 1.00M,
                    MultiBuyOption = new MultiBuyOption
                    {
                        MultiBuyUnitCount = 6,
                        MultiBuyPriceTotal = 5.00M
                    }
                },
                new Product
                {
                    Code = "D",
                    UnitPrice = 0.75M
                }
            };

            ProductList.Products.AddRange(productList);
        }

        public void ScanProduct(string productCode)
        {
            var product = ProductList.Products.SingleOrDefault(p => p.Code == productCode);

            if (product == null)
            {
                throw new ArgumentException($"No product found with product code {productCode}");
            }

            SalesOrder.Products.Add(product);
        }

        public decimal CalculateTotal()
        {
            var orderGroupedByProductCodeCount = SalesOrder.Products.GroupBy(p => p.Code)
                .Select(x => new { Code = x.Key, Count = x.Count() })
                .ToList();

            var cumulativeTotal = 0.0M;

            foreach(var productOrder in orderGroupedByProductCodeCount)
            {
                cumulativeTotal += CalculateProductGroupTotal(productOrder.Code, productOrder.Count);
            }

            return cumulativeTotal;
        }

        private decimal CalculateProductGroupTotal(string code, int count)
        {
            var product = ProductList.Products.SingleOrDefault(p => p.Code == code);
            var productGroupTotal = 0.0M;

            if (product == null) // should never happen as exception should be thrown at scanning stage anyway, but just in case
            {
                return productGroupTotal;
            }

            if (product.MultiBuyOption != null && product.MultiBuyOption.MultiBuyUnitCount <= count)
            {
                var timesToApplyMultiBuyPrice = count / product.MultiBuyOption.MultiBuyUnitCount; // how many iterations of multibuy price to apply
                count %= product.MultiBuyOption.MultiBuyUnitCount; // how many iterations of standard price to apply remaining

                productGroupTotal = timesToApplyMultiBuyPrice * product.MultiBuyOption.MultiBuyPriceTotal;
            }
            return product.UnitPrice * count + productGroupTotal;
        }
    }
}
