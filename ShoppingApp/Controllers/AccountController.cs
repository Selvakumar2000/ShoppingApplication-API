using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Data;
using ShoppingApp.DTOs;
using ShoppingApp.Entities;
using ShoppingApp.Extensions;
using ShoppingApp.Helpers;
using ShoppingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly EmailSender _emailSender;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,TokenService tokenService,
                                 EmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }

        [HttpPost("Register")]
        //api/account
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) 
                return BadRequest("UserName is already taken  Choose Someother Name");

            if (await UserEmailExists(registerDto.Email))
                return BadRequest("Email is already taken  Choose Someother Email");

            //Initialize the mapper
            var config = new MapperConfiguration(cfg => cfg.CreateMap<RegisterDto, AppUser>());

            //Using automapper
            IMapper mapper = config.CreateMapper();

            var user = mapper.Map<RegisterDto, AppUser>(registerDto);

            user.UserName = registerDto.Username;

            if(await SendMailAsync(user.Email, user.UserName))
            {
                user.EmailSent = "Yes";
            }
            else
            {
                user.EmailSent = "No";
            }
                        
            user.IsActive = 1;
            /*GUID stands for Global Unique Identifier.
             *A GUID is a 128-bit integer (16 bytes) that you can use across all computers and networks
             *wherever a unique identifier is required.
             */
            user.UniqueId = Guid.NewGuid().ToString();
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
                UserRole = user.UserRole,
                Gender = user.Gender,
                PhotoUrl = user.PhotoUrl,
                UniqueId = user.UniqueId
            };
        }

        [HttpPost("ResetPasswordLink")]
        public async Task<ActionResult<string>> ChangePassword(PasswordChangeDto details)
        {
            var user = await _userManager.Users.Where(x => x.UserName == details.UserName && x.Email == details.Email)
                                               .FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("User Doesnot Exists");
            }
            
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", details.Email },
                {"username", details.UserName }
            };

            var callback = QueryHelpers.AddQueryString(details.ClientUrl, param);
            var message = new Message(new string[] { user.Email }, "Password Reset For ShopMe Portal", callback);
            
            await _emailSender.SendResetPasswordAsync(message, user.UserName);

            return Ok("Password Reset link has been sent, please check your email.");
        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult<string>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.Users.Where(x => x.UserName == resetPasswordDto.UserName && x.Email == resetPasswordDto.Email)
                                               .FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest("User Doesnot Exists");
            }
                
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);
                return BadRequest("Problem in changing password, try after sometimes");
            }
            return Ok("Password Changed Successfully");
        }

        //make username to be unique
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username);
        }

        //make email to be unique
        private async Task<bool> UserEmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email);
        }

        //send email
        public async Task<bool> SendMailAsync(string email, string username)
        {
            var message = new Message(new string[] { email }, "Welcome To ShopMe Portal",
            "Greetings from ShopMe");
           
            await _emailSender.SendEmailAsync(message, username);
            return true;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            /*UserName Check*/
            var user = await _userManager.Users
                                         .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("UserName is Incorrect");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Password is Incorrect");

            var userStatus = await _userManager.Users.Where(u => u.UserName == loginDto.Username)
                                                     .Select(s => s.IsActive).FirstOrDefaultAsync();

            if (userStatus == 1)
            {
                return new UserDto { };

            }

            user.IsActive = 1;
            user.UniqueId = Guid.NewGuid().ToString();
            await _userManager.UpdateAsync(user);

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                UserRole = user.UserRole,
                Gender = user.Gender,
                PhotoUrl = user.PhotoUrl,
                UniqueId = user.UniqueId
            };
        }

        [HttpGet("Logout/{username}")]
        public async Task<ActionResult> Logout(string username)
        {
            var user = await _userManager.Users
                                         .SingleOrDefaultAsync(x => x.UserName == username);

            user.IsActive = 0;
            await _userManager.UpdateAsync(user);

            return Ok("Logout");

        }

        [HttpPost("Proceed")]
        public async Task<ActionResult<UserDto>> ProceedToLogin(LoginDto loginDto)
        {
            var user = await _userManager.Users
                                         .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            user.IsActive = 1;
            user.UniqueId = Guid.NewGuid().ToString();
            await _userManager.UpdateAsync(user);

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                UserRole = user.UserRole,
                Gender = user.Gender,
                PhotoUrl = user.PhotoUrl,
                UniqueId = user.UniqueId
            };
        }

        [HttpGet("Check/{uniqueId}")]
        public async Task<int> UniqueIdCheck(string uniqueId)
        {
            var UniqueID = await _userManager.Users.Where(u => u.UniqueId == uniqueId)
                                                     .Select(s => s.UniqueId).FirstOrDefaultAsync();
            if(UniqueID == null)
            {
                return 0;
            }

            return 1;
        }
    }
}
