using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ShoppingApp.DTOs;
using ShoppingApp.Entities;
using ShoppingApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        public UserRepository(IConfiguration config)
        {
            _config = config;
        }


        public string GetUsername(string username)
        {
            string Username = "";
            try
            {
                var connectionString = _config.GetConnectionString("ShopingAppCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SpToGetUsername", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Username", username);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    Username = (string)sdr["UserName"];
                }

                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Username;
        }


        public UserDetailsDto GetUsersDetails(string username)
        {
            UserDetailsDto userdetails = new UserDetailsDto();
            try
            {
                var connectionString = _config.GetConnectionString("ShopingAppCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SpToGetUserdetails", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserName", username);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    userdetails.Fullname = (string)sdr["Fullname"];
                    userdetails.UserName = (string)sdr["UserName"];
                    userdetails.Email = (string)sdr["Email"];
                    userdetails.PhoneNumber = (string)sdr["PhoneNumber"];
                    userdetails.City = (string)sdr["City"];
                    userdetails.State = (string)sdr["State"];
                    userdetails.Country = (string)sdr["Country"];
                    userdetails.UserRole = (string)sdr["UserRole"];
                    userdetails.PhotoUrl = (sdr["PhotoUrl"] == DBNull.Value) ? null : (string)sdr["PhotoUrl"];
                }

                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userdetails;
        }

        public int UpdateDetails(AppUser updatedDetails, int userId)
        {
            int i;
            try
            {
                var connectionString = _config.GetConnectionString("ShopingAppCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SpToUpdateUserDetails", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Fullname", updatedDetails.Fullname);
                cmd.Parameters.AddWithValue("@UserName", updatedDetails.UserName);
                cmd.Parameters.AddWithValue("@Email", updatedDetails.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", updatedDetails.PhoneNumber);
                cmd.Parameters.AddWithValue("@City", updatedDetails.City);
                cmd.Parameters.AddWithValue("@State", updatedDetails.State);
                cmd.Parameters.AddWithValue("@Country", updatedDetails.Country);
                cmd.Parameters.AddWithValue("@UserRole", updatedDetails.UserRole);
                cmd.Parameters.AddWithValue("@PhotoUrl", updatedDetails.PhotoUrl);
                cmd.Parameters.AddWithValue("@PublicId", updatedDetails.PublicId);

                i = cmd.ExecuteNonQuery();

                con.Close();

                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetPhotoUrl(int userId)
        {
            string photoUrl = null;
            try
            {
                var connectionString = _config.GetConnectionString("ShopingAppCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SpToGetPhotoUrl", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    photoUrl = (sdr["PhotoUrl"] == DBNull.Value) ? null : (string)sdr["PhotoUrl"];
                }

                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return photoUrl;
        }

        public string GetPhotoId(int userId)
        {
            string photoId = null;
            try
            {
                var connectionString = _config.GetConnectionString("ShopingAppCon");

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SpToGetPhotoId", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    photoId = (sdr["PublicId"] == DBNull.Value) ? null : (string)sdr["PublicId"];
                }

                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return photoId;
        }
    }
}
