using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryScanner.Core.Services.Products.Models
{
    public class Product
    {
        public string Code { get; set; }
        public decimal UnitPrice { get; set; }
        public MultiBuyOption MultiBuyOption { get; set; }
    }

    public class MultiBuyOption
    {
        public int MultiBuyUnitCount { get; set; }
        public decimal MultiBuyPriceTotal { get; set; }
    }
}
