using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private static IWebHostEnvironment _webHostEnvironment;

        public AdminController(IAuthService authService,
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper)
        {
            _authService = authService;
            _userManager = userManager;
            _mapper = mapper;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("createAccount")]
        public async Task<IActionResult> CreateAccount([FromForm] CreateAccountDTO account)
        {
            var accountMap = _mapper.Map<Account>(account);
            accountMap.Id = Guid.NewGuid();
            if (account.AvatarImageFile != null)
            {
                try
                {
                    if (!_userService.IsValidImageFile(account.AvatarImageFile))
                    {
                        return BadRequest("Invalid image file format. Only PNG, JPG, JPEG, and GIF are allowed.");
                    }

                    var imagePaths = await _userService.SaveImage(account.AvatarImageFile, (accountMap.Id).ToString());
                    if (imagePaths.Any())
                    {
                        accountMap.AvatarImagePath = imagePaths;
                    }
                    else
                    {
                        return BadRequest("Failed to save avatar image.");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
            if (await _authService.CreateAccountUser(accountMap))
            {
                return Ok("Create Successful");
            }
            return BadRequest("Something went wrong");
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
                var imageBytes = await _userService.GetImagesByUserId(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (imageBytes == null)
                {
                    var defaultImageFileName = "default-avatar.jpg";
                    var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName); // Provide the path to your default image
                    imageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                }

                accountDto.Add(new
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Role = roles,
                    FacultyId = user.FacultyId,
                    ImageAvatarBytes = imageBytes
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
                if (!string.IsNullOrEmpty(user.AvatarImagePath))
                {
                    var avatarDirectory = Path.GetDirectoryName(Path.Combine(_webHostEnvironment.WebRootPath, user.AvatarImagePath?.TrimStart('\\')));
                    if (Directory.Exists(avatarDirectory))
                    {
                        Directory.Delete(avatarDirectory, true);
                    }
                }

                return Ok(); // Successfully deleted
            }
            else
            {
                return BadRequest(result.Errors); // Failed to delete user
            }
        }


    }
}
