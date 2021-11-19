using ShoppingApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Interfaces
{
    public interface IOrderManagement
    {
        public int AddToCart(AddToCartDto productDetails);
        public string GetProductGender(int productId);
        public int GetProductStatus(int productId, int buyerId, string buyerName, string buyerRole);
        public List<ProductsDto> GetCartProducts(int buyerId);
    }
}
