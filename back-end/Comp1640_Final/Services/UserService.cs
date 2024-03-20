using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Comp1640_Final.Services
{
    public interface IUserService
    {
        bool IsValidImageFile(IFormFile imageFile);
        Task<string> SaveImage(IFormFile imageFile, string subFolderName);
        Task<byte[]> GetImagesByUserId(string userId);

    }
    public class UserService : IUserService
    {
        private readonly ProjectDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(ProjectDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        
        public bool IsValidImageFile(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return false;
            }

            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".gif" }; // Add more file extensions as needed

            // Get the file extension
            var fileExtension = Path.GetExtension(imageFile.FileName);

            // Check if the file extension is in the allowed extensions list
            if (!allowedExtensions.Contains(fileExtension?.ToLower()))
            {
                return false;
            }

            // Perform additional checks if needed, such as content type validation.

            return true;
        }

        public async Task<string> SaveImage(IFormFile imageFile, string subfolderName)
        {
            if (!IsValidImageFile(imageFile))
            {
                throw new ArgumentException("Invalid image file format. Only PNG, JPG, JPEG, and GIF are allowed.");
            }

            var imagesDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", subfolderName);

            if (!Directory.Exists(imagesDirectory))
                Directory.CreateDirectory(imagesDirectory);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(imagesDirectory, fileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            return "\\UserAvatars\\" + subfolderName + "\\" + fileName;
        }

        public async Task<byte[]> GetImagesByUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return null; // Or handle as needed
            }

            var imagePath = user.AvatarImagePath;

            if (string.IsNullOrEmpty(imagePath))
            {
                return null; // Or handle as needed if the image path is empty
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath?.TrimStart('\\'));

            if (System.IO.File.Exists(filePath))
            {
                var imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return imageBytes;
            }
            else
            {
                // Optionally log or handle the case where the image file is not found
                return null;
            }
        }
    }
}
