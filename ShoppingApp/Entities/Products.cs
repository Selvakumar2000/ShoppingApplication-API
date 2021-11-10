using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Entities
{
    public class Products
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string ProductBrand { get; set; }
        public string ProductDescription { get; set; }
        public string AmountRs { get; set; }
        public int OriginalPrice { get; set; }
        public int Discount { get; set; }
        public string Category { get; set; }
        public string Gender { get; set; }
        public string PhotoUrl { get; set; }
        public string PhotoPublicId { get; set; }
    }
}