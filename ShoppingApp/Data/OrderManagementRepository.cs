using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ShoppingApp.DTOs;
using ShoppingApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Data
{
    public class OrderManagementRepository : IOrderManagement
    {
        private readonly IConfiguration _config;
        public OrderManagementRepository(IConfiguration config)
        {
            _config = config;
        }
        public int AddToCart(AddToCartDto productDetails)
        {
            var connectionString = _config.GetConnectionString("ShopingAppCon");

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("SpToAddProductCart", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@BuyerId", productDetails.BuyerId);
            cmd.Parameters.AddWithValue("@BuyerName", productDetails.BuyerName);
            cmd.Parameters.AddWithValue("@BuyerRole", productDetails.BuyerRole);
            cmd.Parameters.AddWithValue("@ProductId", productDetails.ProductId);
            cmd.Parameters.AddWithValue("@ProductName", productDetails.ProductName);
            cmd.Parameters.AddWithValue("@ProductBrand", productDetails.ProductBrand);
            cmd.Parameters.AddWithValue("@ProductDescription", productDetails.ProductDescription);
            cmd.Parameters.AddWithValue("@AmountRs", productDetails.AmountRs);
            cmd.Parameters.AddWithValue("@Discount", productDetails.Discount);
            cmd.Parameters.AddWithValue("@Category", productDetails.Category);
            cmd.Parameters.AddWithValue("@ProductGender", productDetails.ProductGender);
            cmd.Parameters.AddWithValue("@PhotoUrl", productDetails.PhotoUrl);
            cmd.Parameters.AddWithValue("@AddedTime", productDetails.AddedTime);
            cmd.Parameters.AddWithValue("@ProductStatus", productDetails.ProductStatus);

            int i = cmd.ExecuteNonQuery();
            con.Close();

            return i;
        }

        public string GetProductGender(int productId)
        {
            string productGender = "";
            try
            {
                var connectionString = _config.GetConnectionString("ShopingAppCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SpToGetProductGender", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ProductId", productId);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    productGender = (string)sdr["Gender"];
                }

                con.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong....." + ex.Message);
            }

            return productGender;
        }

        public int GetProductStatus(int productId, int buyerId, string buyerName, string buyerRole)
        {
            int status = 0;
            try
            {
                var connectionString = _config.GetConnectionString("ShopingAppCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SpToGetProductStatus", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@BuyerId", buyerId);
                cmd.Parameters.AddWithValue("@BuyerName", buyerName);
                cmd.Parameters.AddWithValue("@BuyerRole", buyerRole);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    status = (int)sdr["RecordExists"];
                }

                con.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong....." + ex.Message);
            }

            return status;
        }
    }
}
