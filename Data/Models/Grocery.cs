using System;
using System.Collections.Generic;

namespace Data.Models
{
    public partial class Grocery
    {
        public int GroceryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }

        public string Category { get; set; }
        public decimal Price { get; set; }
        public string CoverFileName { get; set; }
    }
}
