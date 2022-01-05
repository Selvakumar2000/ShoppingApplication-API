using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShoppingApp.Data;
using ShoppingApp.DTOs;
using ShoppingApp.Entities;
using ShoppingApp.Extensions;
using ShoppingApp.Helpers;
using ShoppingApp.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    /*
     * Now this API is protected, we need to authenticate with authentication scheme (Ex. jwtBearer)
     * Add authentication scheme service into our application
     * Add Authentication Middleware which must be above the authorization middleware
     * So in order to authorize our request, we need to send the jwt token along the request
     * Send the token in Headers prefix with Bearer
     * 
     * In c#, the readonly fields can be initialized either at the declaration or in a constructor.
     * The readonly field values will be evaluated during the run time in c#. Once values are assigned to read-only
     * fields, then those values must be the same throughout the application.
     */
    public class ProductsController : BaseApiController
    {       
        private readonly ProductsRepository _productsRepository;
        private readonly ProductPhotoService _productPhotoService;
        public ProductsController(ProductsRepository productsRepository, ProductPhotoService productPhotoService)
        {
            _productsRepository = productsRepository;
            _productPhotoService = productPhotoService;
        }


        [HttpGet]           //[FromQuery] is used for handle the empty queryparameters
        public ActionResult<IEnumerable<ProductsDto>> GetProducts([FromQuery] UserParams userParams)
        {
            if(string.Equals(User.GetUserRole(), "Buyer") || string.Equals(User.GetUserRole(), "GoldBuyer"))
            {
                var users = _productsRepository.GetProducts(userParams);
                Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
                return Ok(users);
            }

            return Unauthorized("You are not allowed here");
        }


        //Ref --> https://stackoverflow.com/questions/41367602/upload-files-and-json-in-asp-net-core-web-api

        [HttpPost("upload-product")]
        public ActionResult<string> AddProduct([FromForm] string productDetails, [FromForm] IFormFile file)
        {
            int i;
            if (string.Equals(User.GetUserRole(), "Supplier") || string.Equals(User.GetUserRole(), "GoldSupplier"))
            {
                ProductDetailsDto ProductDetails = JsonConvert.DeserializeObject<ProductDetailsDto>(productDetails);
                try
                {
                    ImageUploadResult result = _productPhotoService.AddProductPhoto(file);
                    if (result.Error != null) return BadRequest(result.Error.Message + "Problem adding Product Image");

                    ProductDetails.SupplierId = User.GetUserId();
                    ProductDetails.SupplierName = User.GetUsername();
                    ProductDetails.OriginalPrice = ProductDetails.AmountRs - 200;
                    
                    i = _productsRepository.AddProduct(ProductDetails, result);

                }
                catch (Exception ex)
                {
                    return BadRequest("Operation Failed  " + ex);
                }
            }
            else
            {
                return BadRequest("You are not allowed here");
            }
            
            if (i > 0)
            {
                return Ok("Product Details Addedd Successfully");
            }

            return BadRequest("Problem in Adding Product Details");
        }

        [HttpGet("uploadedproducts")]
        public ActionResult<List<ProductsDto>> GetUploadedProducts()
        {
            int supplierId = User.GetUserId();
            List<ProductsDto> products = _productsRepository.GetUploadedProducts(supplierId);
            return Ok(products);
        }

        [HttpPut("updateproduct")]
        public ActionResult<string> UpdateProdcutDetails(UpdateProduct productDetails)
        {
            int i = 1;

            if (i == _productsRepository.UpdateProdcutDetails(productDetails))
            {
                return Ok("Product Details Updated Successfully");
            }

            return BadRequest("Problem in update product details");
        }

        [HttpDelete("deleteProduct/{productId}")]
        public ActionResult<string> DeleteProduct(int productId)
        {
            int i = 1;

            if (i == _productsRepository.DeleteProduct(productId))
            {
                return Ok("Product Removed Successfully");
            }

            return BadRequest("Problem in Remove Product");
        }

    }
}
