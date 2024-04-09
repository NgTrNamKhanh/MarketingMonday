using AutoMapper;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
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
        private readonly Cloudinary _cloudinary;
        private readonly ICloudinaryService _cloudinaryService;

        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(IUserService userService,
            IMapper mapper,
            ProjectDbContext context,
            IWebHostEnvironment webHostEnvironment,
            Cloudinary cloudinary,
            ICloudinaryService cloudinaryService,
            UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _cloudinary = cloudinary;
            _cloudinaryService = cloudinaryService;
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
            var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

            // If imageBytes is null, read the default image file
            if (cloudUserImage == null)
            {
                var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                cloudUserImage = defaultImageFileName;
            }

            var accountDto = new
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Role = roles,
                FacultyId = user.FacultyId,
                CloudAvatar = cloudUserImage
            };

            return Ok(accountDto);
        }
        [HttpPut("ChangeAvatar")]
        public async Task<ActionResult<Models.Account>> ChangeAvatar(IFormFile avatarImage, string userId)
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
                if (user.CloudAvatarImagePath != null)
                {
                    var oldImagePublicId = _userService.GetPublicIdFromImageUrl(user.CloudAvatarImagePath); // Extract public ID of the old image

                    // Delete the old image from Cloudinary
                    if (!string.IsNullOrEmpty(oldImagePublicId))
                    {
                        await _cloudinaryService.DeleteResource(oldImagePublicId);
                    }
                }
                // Upload the new image to Cloudinary using the service method
                var uploadResult = await _cloudinaryService.UploadAvatarImage(avatarImage);

                // Update the user's avatar image URL with the new one from Cloudinary
                user.CloudAvatarImagePath = uploadResult.Uri.ToString();

                // Update the user entity in the database
                await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to change avatar: {ex.Message}");
            }

            return Ok(user.CloudAvatarImagePath);
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
