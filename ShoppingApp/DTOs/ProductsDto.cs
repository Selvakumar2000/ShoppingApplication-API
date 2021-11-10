using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.DTOs
{
    public class ProductsDto
    {
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public string ProductDescription { get; set; }
        public int AmountRs { get; set; }
        public int OriginalPrice { get; set; }
        public int Discount { get; set; }
        public string PhotoUrl { get; set; }
    }
}
