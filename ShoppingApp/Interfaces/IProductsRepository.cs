using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.DTOs;
using ShoppingApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Interfaces
{
    public interface IProductsRepository
    {
        public int AddProduct(ProductDetailsDto productDetails, ImageUploadResult result);
        public PagedList<ProductsDto> GetProducts(UserParams userParams);
        public List<ProductsDto> GetUploadedProducts(int supplierId);

    }
}
