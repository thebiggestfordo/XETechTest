using GroceryScanner.Core.Services.Products.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryScanner.Core.Services
{
    public interface IPointOfSaleTerminalService
    {
        void SetPricing();
        void ScanProduct(string productCode);
        decimal CalculateTotal();
    }
}
