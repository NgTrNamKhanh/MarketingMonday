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

namespace Comp1640_Final.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IMapper _mapper;
        private readonly IUserService _userService;
        //private static IWebHostEnvironment _webHostEnvironment;
        private readonly Cloudinary _cloudinary;

        public AdminController(IAuthService authService,
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            //IWebHostEnvironment webHostEnvironment,
            Cloudinary cloudinary
            //IMapper mapper
            )
        {
            _authService = authService;
            _userManager = userManager;
            //_mapper = mapper;
            _userService = userService;
            _cloudinary = cloudinary;
            //_webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("createAccount")]
        public async Task<IActionResult> CreateAccount([FromForm] Models.Account account)
        {
            //var accountMap = _mapper.Map<Models.Account>(account);
            //accountMap.Id = Guid.NewGuid();

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

        //[HttpPut]
        //public async Task<IActionResult> PutAccount(string email, string password)
        //{
        //    //name = account.Email;
        //    var user = await _userManager.FindByEmailAsync(email);
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var result = await _userManager.ResetPasswordAsync(user, token, password);
        //    if (result.Succeeded)
        //    {
        //        return Ok("Successful");
        //    }
        //    return BadRequest("Failed");
        //}

        [HttpPut("account")]
        public async Task<IActionResult> PutAccountForAdmin([FromForm] EditAccountDTO account, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var oldRoles = await _userManager.GetRolesAsync(user);
            if (!string.IsNullOrWhiteSpace(account.Password))
            {
                // If password is provided and not empty, reset it
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, account.Password);
                if (!result.Succeeded)
                {
                    return BadRequest("Failed to reset password");
                }
            }

            user.Email = account.Email;
            user.PhoneNumber = account.PhoneNumber;
            user.UserName = account.Email;
            user.FirstName = account.FirstName;
            user.LastName = account.LastName;
            user.FacultyId = account.FacultyId;
            await _userManager.RemoveFromRolesAsync(user, oldRoles);
            // Thêm vai trò mới
            var changeRole = await _userManager.AddToRoleAsync(user, account.Role);
            var changeEmail = await _userManager.UpdateAsync(user);
            if (changeEmail.Succeeded)
            {
                return Ok("Successful");
            }
            return BadRequest("Failed");
        }

        [HttpGet("accounts")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
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

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(); // User not found
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(); // Successfully deleted
            }
            else
            {
                return BadRequest(result.Errors); // Failed to delete user
            }
        }


    }
}
