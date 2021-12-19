using System.Collections.Generic;

namespace GroceryScanner.Core.Services.Products.Models
{
    public static class ProductList
    {
        static ProductList()
        {
            Products = new List<Product>();
        }

        public static List<Product> Products { get; set; }
    }
}
