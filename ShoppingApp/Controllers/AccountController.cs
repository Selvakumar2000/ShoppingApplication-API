using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.DTOs;
using ShoppingApp.Entities;
using ShoppingApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
                                ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        //api/account
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) 
                return BadRequest("UserName is already taken  Choose Someother Name");

            //Initialize the mapper
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RegisterDto, AppUser>());

            //Using automapper
            IMapper mapper = config.CreateMapper();

            var user = mapper.Map<RegisterDto, AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            IdentityResult roleResult;

            if (registerDto.UserRole == "Buyer")
            {
                roleResult = await _userManager.AddToRoleAsync(user, "Buyer");
            }
            else if (registerDto.UserRole == "GoldBuyer")
            {
                roleResult = await _userManager.AddToRoleAsync(user, "GoldBuyer");
            }
            else if (registerDto.UserRole == "Supplier")
            {
                roleResult = await _userManager.AddToRoleAsync(user, "Supplier");
            }
            else if (registerDto.UserRole == "GoldSupplier")
            {
                roleResult = await _userManager.AddToRoleAsync(user, "GoldSupplier");
            }
            else
            {
                roleResult = await _userManager.AddToRoleAsync(user, "Buyer");
            }

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Fullname = user.Fullname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserRole = user.UserRole,
                Gender = user.Gender
            };
        }


        //make username to be unique
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            /*UserName Check*/
            var user = await _userManager.Users
                                         .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("UserName is Incorrect");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Password is Incorrect");

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Fullname = user.Fullname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserRole = user.UserRole,
                Gender = user.Gender
            };
        }
    }
}
