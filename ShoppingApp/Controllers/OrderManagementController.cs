﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingApp.DTOs;
using ShoppingApp.Extensions;
using ShoppingApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderManagementController : BaseApiController
    {
        private readonly IOrderManagement _orderManagement;
        public OrderManagementController(IOrderManagement orderManagement)
        {
            _orderManagement = orderManagement;
        }


        [HttpPost("addto-cart")]
        public ActionResult<string> AddToCart(AddToCartDto productDetails)
        {
            int spStatus, status = 1, buyerId;
            string buyerName, buyerRole;

            buyerId = User.GetUserId();
            buyerName = User.GetUsername();
            buyerRole = User.GetUserRole();

            spStatus = _orderManagement.GetProductStatus(productDetails.ProductId, buyerId, buyerName, buyerRole);

            if (status == spStatus)
            {
                return Ok("This Product Already In Your Cart Section");
            }

            AddToCartDto obj = new AddToCartDto();

            int i;
            if (string.Equals(User.GetUserRole(), "Buyer") || string.Equals(User.GetUserRole(), "GoldBuyer"))
            {
                try
                {
                    obj.BuyerId = buyerId;
                    obj.BuyerName = buyerName;
                    obj.BuyerRole = buyerRole;
                    obj.ProductGender = _orderManagement.GetProductGender(productDetails.ProductId);
                    obj.AddedTime = DateTime.Now;
                    obj.ProductStatus = 0;
                    obj.ProductId = productDetails.ProductId;
                    obj.ProductName = productDetails.ProductName;
                    obj.ProductBrand = productDetails.ProductBrand;
                    obj.ProductDescription = productDetails.ProductDescription;
                    obj.AmountRs = productDetails.AmountRs;
                    obj.Discount = productDetails.Discount;
                    obj.Category = productDetails.Category;
                    obj.PhotoUrl = productDetails.PhotoUrl;

                    i = _orderManagement.AddToCart(obj);

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
                return Ok("Product Addedd To Your Cart Section Successfully");
            }

            return BadRequest("Problem in Adding Product Into Your Cart Section");
        }
    }
}