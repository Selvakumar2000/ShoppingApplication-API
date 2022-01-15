using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShoppingApp.DTOs;
using ShoppingApp.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Data
{
    public class ProductsRepository 
    {
        private readonly IConfiguration _config;
        public ProductsRepository(IConfiguration config)
        {
            _config = config;
        }

        public int AddProduct(ProductDetailsDto ProductDetails, ImageUploadResult result)
        {
            var connectionString = _config.GetConnectionString("ShopingAppCon");

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("AddProduct", con)
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
            cmd.Parameters.AddWithValue("Quantity", ProductDetails.Quantity);

            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i;
        }


        public PagedList<ProductsDto> GetProducts(UserParams userParams)
        {
            List<ProductsDto> productsList = new List<ProductsDto>();

            try
            {
                var connectionString = _config.GetConnectionString("ShopingAppCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("GetProducts", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Category", userParams.Category);
                cmd.Parameters.AddWithValue("@MinPrice", userParams.MinPrice);
                cmd.Parameters.AddWithValue("@MaxPrice", userParams.MaxPrice);
                cmd.Parameters.AddWithValue("@Gender", userParams.Gender);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    ProductsDto products = new ProductsDto
                    {
                        ProductId = (int)sdr["ProductId"],
                        ProductName = (string)sdr["ProductName"],
                        ProductBrand = (string)sdr["ProductBrand"],
                        ProductDescription = (string)sdr["ProductDescription"],
                        AmountRs = (int)sdr["AmountRs"],
                        OriginalPrice = (int)sdr["OriginalPrice"],
                        Discount = (int)sdr["Discount"],
                        PhotoUrl = (string)sdr["PhotoUrl"],
                        Category= (string)sdr["Category"]
                    };

                    productsList.Add(products);

                }

                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return  PagedList<ProductsDto>
                    .Create(productsList, userParams.PageNumber, userParams.PageSize);
        }

        public List<ProductsDto> GetUploadedProducts(int supplierId)
        {
            List<ProductsDto> productsList = new List<ProductsDto>();

            try
            {
                var connectionString = _config.GetConnectionString("ShopingAppCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("GetUploadedProducts", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@SupplierId", supplierId);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    ProductsDto products = new ProductsDto
                    {
                        ProductId = (int)sdr["ProductId"],
                        ProductName = (string)sdr["ProductName"],
                        ProductBrand = (string)sdr["ProductBrand"],
                        ProductDescription = (string)sdr["ProductDescription"],
                        AmountRs = (int)sdr["AmountRs"],
                        OriginalPrice = (int)sdr["OriginalPrice"],
                        Discount = (int)sdr["Discount"],
                        PhotoUrl = (string)sdr["PhotoUrl"],
                        Category = (string)sdr["Category"],
                        Quantity = (int)sdr["Quantity"]
                    };

                    productsList.Add(products);

                }

                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return productsList;
        }

        public int UpdateProdcutDetails(UpdateProduct productDetails)
        {
            int i;
            try
            {
                var connectionString = _config.GetConnectionString("ShopingAppCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("UpdateProductDetails", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ProductId", productDetails.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", productDetails.ProductName);
                cmd.Parameters.AddWithValue("@ProductDescription", productDetails.ProductDescription);
                cmd.Parameters.AddWithValue("@AmountRs", productDetails.AmountRs);
                cmd.Parameters.AddWithValue("@Discount", productDetails.Discount);
                cmd.Parameters.AddWithValue("@Quantity", productDetails.Quantity);

                i = cmd.ExecuteNonQuery();

                con.Close();

                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteProduct(int productId)
        {
            int i;
            try
            {
                var connectionString = _config.GetConnectionString("ShopingAppCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("DeleteProduct", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ProductId", productId);

                i = cmd.ExecuteNonQuery();

                con.Close();

                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
