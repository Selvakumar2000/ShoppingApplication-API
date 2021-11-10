using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShoppingApp.DTOs;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductPhotoService _productPhotoService;
        private readonly IConfiguration _config;
        public ProductsController(IProductPhotoService productPhotoService, IConfiguration config)
        {
            _productPhotoService = productPhotoService;
            _config = config;
        }


        //Ref --> https://stackoverflow.com/questions/41367602/upload-files-and-json-in-asp-net-core-web-api

        [HttpPost("add-product")]
        public ActionResult<string> AddProduct([FromForm] string productDetails, [FromForm] IFormFile file)
        {
            int i;
            ProductDetailsDto ProductDetails = JsonConvert.DeserializeObject<ProductDetailsDto>(productDetails);
            try
            {
                var result = _productPhotoService.AddProductPhoto(file);
                if (result.Error != null) return BadRequest(result.Error.Message + "Problem adding Product Image");

                var connectionString = _config.GetConnectionString("ShopingAppCon");
                
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SpToAddProduct", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ProductName", ProductDetails.ProductName);
                cmd.Parameters.AddWithValue("@SupplierId", ProductDetails.SupplierId);
                cmd.Parameters.AddWithValue("@SupplierName", ProductDetails.SupplierName);
                cmd.Parameters.AddWithValue("@ProductBrand", ProductDetails.ProductBrand);
                cmd.Parameters.AddWithValue("@ProductDescription", ProductDetails.ProductDescription);
                cmd.Parameters.AddWithValue("@AmountRs", ProductDetails.AmountRs);
                cmd.Parameters.AddWithValue("@OriginalPrice", ProductDetails.OriginalPrice);
                cmd.Parameters.AddWithValue("@Discount", ProductDetails.Discount);
                cmd.Parameters.AddWithValue("@Category", ProductDetails.Category);
                cmd.Parameters.AddWithValue("@Gender", ProductDetails.Gender);
                cmd.Parameters.AddWithValue("@PhotoUrl", result.SecureUrl.AbsoluteUri);
                cmd.Parameters.AddWithValue("@PhotoPublicId", result.PublicId);

                i = cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                return BadRequest("Operation Failed  " + ex);
            }

            if(i > 0)
            {
                return Ok("Product Details Addedd Successfully");
            }

            return BadRequest("Problem in Adding Product Details");
        }

        [HttpGet("{category}")]
        public ActionResult<List<ProductsDto>> GetProducts(string category)
        {
            List<ProductsDto> productsList = new List<ProductsDto>();

            try
            {
                var connectionString = _config.GetConnectionString("ShopingAppCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SpToGetProducts", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Gender", "Male");
                cmd.Parameters.AddWithValue("@Category", category);
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    ProductsDto products = new ProductsDto();

                    products.ProductName = (string)sdr["ProductName"];
                    products.ProductBrand = (string)sdr["ProductBrand"];
                    products.ProductDescription = (string)sdr["ProductDescription"];
                    products.AmountRs = (int)sdr["AmountRs"];
                    products.OriginalPrice = (int)sdr["OriginalPrice"];
                    products.Discount = (int)sdr["Discount"];
                    products.PhotoUrl = (string)sdr["PhotoUrl"];

                    productsList.Add(products);
                                                                          
                }
                
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong....." + ex.Message);
            }

            //return await PagedList<ProductsDto>.Create(productsList, userParams.PageNumber, userParams.PageSize);
            return productsList;
        }
    }
}
