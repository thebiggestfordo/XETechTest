using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryScanner.Core.Services.Products.Models
{
    public static class SalesOrder
    {
        static SalesOrder()
        {
            Products = new List<Product>();
        }

        public static List<Product> Products { get; set; }
    }
}
