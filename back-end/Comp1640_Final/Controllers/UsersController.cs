using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Comp1640_Final.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ProjectDbContext _context;
        private static IWebHostEnvironment _webHostEnvironment;

        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(IUserService userService,
            IMapper mapper,
            ProjectDbContext context,
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        [HttpGet("accounts/{Id}")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUserById(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound(); // User not found
            }

            var roles = await _userManager.GetRolesAsync(user);
            var imageBytes = await _userService.GetImagesByUserId(user.Id);

            var accountDto = new
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Role = roles,
                FacultyId = user.FacultyId,
                ImageAvatarBytes = imageBytes
            };

            return Ok(accountDto);
        }
        [HttpPut("ChangeAvatar")]
        public async Task<ActionResult<Account>> ChangeAvatar(IFormFile avatarImage, string userId)
        {
            if (avatarImage == null || avatarImage.Length == 0)
                return BadRequest("No image file provided.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(userId);

            try
            {
                if (!_userService.IsValidImageFile(avatarImage))
                {
                    return BadRequest("Invalid image file format. Only PNG, JPG, JPEG, and GIF are allowed.");
                }

                var oldImagePath = user.AvatarImagePath; // Store the old image path

                var imagePath = await _userService.SaveImage(avatarImage, (user.Id).ToString());

                // Delete the old image file
                if (!string.IsNullOrEmpty(oldImagePath))
                {
                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, oldImagePath.TrimStart('\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Set the user's avatar image path to the new one
                user.AvatarImagePath = imagePath;
                
                await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to change avatar: {ex.Message}");
            }
            var imageBytes = await _userService.GetImagesByUserId(userId);
            return Ok(imageBytes);
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> PutAccount(string email, string password)
        {
            //name = account.Email;
            var user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (result.Succeeded)
            {
                return Ok("Successful");
            }
            return BadRequest("Failed");
        }
    }
}
