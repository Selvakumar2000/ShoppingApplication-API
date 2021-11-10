using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShoppingApp.DTOs;
using ShoppingApp.Extensions;
using ShoppingApp.Helpers;
using ShoppingApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseApiController
    {       
        private readonly IProductsRepository _productsRepository;
        private readonly IProductPhotoService _productPhotoService;
        public ProductsController(IProductsRepository productsRepository, IProductPhotoService productPhotoService)
        {
            _productsRepository = productsRepository;
            _productPhotoService = productPhotoService;
        }


        //Ref --> https://stackoverflow.com/questions/41367602/upload-files-and-json-in-asp-net-core-web-api

        [HttpPost("add-product")]
        public ActionResult<string> AddProduct([FromForm] string productDetails, [FromForm] IFormFile file)
        {
            int i;
            ProductDetailsDto ProductDetails = JsonConvert.DeserializeObject<ProductDetailsDto>(productDetails);
            try
            {
                ImageUploadResult result = _productPhotoService.AddProductPhoto(file);
                if (result.Error != null) return BadRequest(result.Error.Message + "Problem adding Product Image");

                i = _productsRepository.AddProduct(ProductDetails, result);

            }
            catch (Exception ex)
            {
                return BadRequest("Operation Failed  " + ex);
            }

            if (i > 0)
            {
                return Ok("Product Details Addedd Successfully");
            }

            return BadRequest("Problem in Adding Product Details");
        }

        [HttpGet]           //[FromQuery] is used for handle the empty queryparameters
        public ActionResult<IEnumerable<ProductsDto>> GetProductsAsync([FromQuery] UserParams userParams)
        {
            var users =_productsRepository.GetProducts(userParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }
    }
}
