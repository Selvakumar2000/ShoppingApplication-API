using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.DTOs
{
    public class ProductDetailsDto
    {
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string ProductBrand { get; set; }
        public string ProductDescription { get; set; }
        public int AmountRs { get; set; }
        public int OriginalPrice { get; set; }
        public int Discount { get; set; }
        public string Category { get; set; }
        public string Gender { get; set; }
    }
}
