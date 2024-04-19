using AutoMapper;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Comp1640_Final.DTO;
using Comp1640_Final.IServices;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Comp1640_Final.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly Cloudinary _cloudinary;

        public AdminController(IAuthService authService,
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            Cloudinary cloudinary
            )
        {
            _authService = authService;
            _userManager = userManager;
            _userService = userService;
            _cloudinary = cloudinary;
        }

        [HttpPost("createAccount")]
        public async Task<IActionResult> CreateAccount([FromForm] Models.Account account)
        {
            try
            {
                if (account.AvatarImageFile != null)
                {
                    if (!_userService.IsValidImageFile(account.AvatarImageFile))
                    {
                        return BadRequest("Invalid image file format. Only PNG, JPG, JPEG, and GIF are allowed.");
                    }

                    // Upload the avatar image to Cloudinary
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(account.AvatarImageFile.FileName, account.AvatarImageFile.OpenReadStream())
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    // Set the user's avatar image path to the URL from Cloudinary
                    account.CloudAvatarImagePath = uploadResult.Uri.ToString();
                }
                else
                {
                    // If no avatar image is provided, set a default image URL
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712939986/tbzbwhyipuf7b4ep6dlm.jpg";
                    account.CloudAvatarImagePath = defaultImageFileName;
                }

                if (await _authService.CreateAccountUser(account))
                {
                    return Ok("Create Successful");
                }
                return BadRequest("Something went wrong");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create account: {ex.Message}");
            }
        }


        [HttpPut("account")]
        public async Task<IActionResult> PutAccountForAdmin([FromForm] EditAccountDTO account, string userId)
        {
            
            if (await _authService.EditAccount(account,userId))
            {
                return Ok("Successful");
            }
            return BadRequest("Failed");
        }

        [HttpGet("accounts")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            var users = _authService.GetAllUsers();
            var accountDto = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                accountDto.Add(new
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Role = roles,
                    FacultyId = user.FacultyId,
                    CloudAvatarImagePath = user.CloudAvatarImagePath,
                });
            }

            return Ok(accountDto);
        }

        [HttpGet("getByName")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUserByName(string name)
        {
            var users = await _authService.GetAccountByName(name);
            var accountDto = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                accountDto.Add(new
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Role = roles,
                    FacultyId = user.FacultyId,
                    CloudAvatarImagePath = user.CloudAvatarImagePath,
                });
            }
            return Ok(accountDto);
        }



        //[HttpDelete("{userId}")]
        //public async Task<IActionResult> DeleteUser(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);

        //    if (user == null)
        //    {
        //        return NotFound(); // User not found
        //    }

        //    var result = await _userManager.DeleteAsync(user);
        //    if (result.Succeeded)
        //    {
        //        return Ok(); // Successfully deleted
        //    }
        //    else
        //    {
        //        return BadRequest(result.Errors); // Failed to delete user
        //    }
        //}


    }
}
