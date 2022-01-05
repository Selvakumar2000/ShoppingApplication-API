using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShoppingApp.Data;
using ShoppingApp.DTOs;
using ShoppingApp.Entities;
using ShoppingApp.Extensions;
using ShoppingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly UserRepository _userRepository;
        private readonly ProductPhotoService _productPhotoService;
        public UsersController(UserRepository userRepository, ProductPhotoService productPhotoService)
        {
            _userRepository = userRepository;
            _productPhotoService = productPhotoService;
        }

        [HttpGet("userdetails")]
        public ActionResult<UserDetailsDto> GetUserDetails()
        {
            string username = User.GetUsername();

            var user = _userRepository.GetUsername(username);

            if (user == null)
            {
                return BadRequest("User Doesnot Exists");
            }

            UserDetailsDto userdetails = _userRepository.GetUsersDetails(username);

            return Ok(userdetails);
        }

        [HttpPut("updatedetails")]
        public ActionResult<UserDetailsDto> UpdateUser([FromForm] string userDetails, [FromForm] IFormFile file)
        {
            UserDetailsDto UserDetails = JsonConvert.DeserializeObject<UserDetailsDto>(userDetails);

            int i = 1;
            int userId = User.GetUserId();
            string username = User.GetUsername();
            ImageUploadResult result;
            string photoUrl = null, photoId = null;

            int status = _userRepository.GetUserExistance(username, UserDetails.PhoneNumber, UserDetails.Email);

            if(status == 1)
            {
                return BadRequest("Email or PhoneNumber is Already Exists");
            }

            photoUrl =  _userRepository.GetPhotoUrl(userId);
            photoId = _userRepository.GetPhotoId(userId);

            if (file != null)
            {
                result = _productPhotoService.AddProductPhoto(file);
                if (result.Error != null) return BadRequest(result.Error.Message + "Problem adding Product Image");
                photoUrl = result.SecureUrl.AbsoluteUri;
                photoId = result.PublicId;
            }

            UserDetails.PhotoUrl = photoUrl;
            UserDetails.PublicId = photoId;

            //Initialize the mapper
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDetailsDto, AppUser>());

            //Using automapper
            IMapper mapper = config.CreateMapper();

            AppUser updatedDetails = mapper.Map<UserDetailsDto, AppUser>(UserDetails);

            if(i == _userRepository.UpdateDetails(updatedDetails, userId))
            {
                UserDetailsDto userdetails = _userRepository.GetUsersDetails(username);
                return Ok(userdetails);
            }

            return Ok("Problem in update your details");
        }
    }
}
