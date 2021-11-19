using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.DTOs
{
    public class AddToCartDto
    {
        public int ProductId{ get; set; }
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public string ProductDescription { get; set; }
        public int AmountRs { get; set; }
        public int Discount { get; set; }
        public string Category { get; set; }
        public int BuyerId { get; set; }
        public string BuyerName { get; set; }
        public string BuyerRole { get; set; }
        public string ProductGender { get; set; }
        public string PhotoUrl{ get; set; }
        public DateTime AddedTime { get; set; }
        public int ProductStatus { get; set; }

    }
}
