using ShoppingApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Interfaces
{
    public interface IOrderManagement
    {
        int AddToCart(AddToCartDto productDetails);
        string GetProductGender(int productId);
        int GetProductStatus(int productId, int buyerId, string buyerName, string buyerRole);
    }
}
