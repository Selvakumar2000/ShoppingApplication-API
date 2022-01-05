using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.DTOs
{
    public class UpdateProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int AmountRs { get; set; }
        public int Discount { get; set; }
        public int Quantity{ get; set; }
    }
}
