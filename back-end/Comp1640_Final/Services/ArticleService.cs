using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Comp1640_Final.Services
{
    public interface IArticleService
    {
        ICollection<Article> GetArticles();
        Article GetArticleByID(Guid id);
        Task<IEnumerable<Article>> GetArticlesByTitle(string title);
        Task<IEnumerable<Article>> GetArticlesByFacultyId(int facultyID);
        Task<IEnumerable<Article>> GetArticleByPublishStatusIdAndFacultyId(int publishStatusId, int facultyId);
        Task<IEnumerable<Article>> GetApprovedArticles(int facultyID);
        Task<IEnumerable<Article>> GetGuestApprovedArticles(int facultyID);
        Task<IEnumerable<Article>> GetArticleByCoordinatorStatusAndFacultyId(bool coordinatorStatus, int facultyId);
        Task<IEnumerable<Article>> GetArticleByProfile(string userId);
        Task<IEnumerable<Article>> GetArticleByPublishStatus(int publishStatusId);
        Task<bool> UpdateArticle(Article article);
        Task<bool> AddArticle(Article article);
        Task<ImageUploadResult> UploadImage(ImageUploadParams parameters);
        Task<RawUploadResult> UploadFile(RawUploadParams parameters);
        bool IsValidImageFile(List<IFormFile> imageFile);
        bool IsValidDocFile(IFormFile imageFile);
        bool ArticleExists(Guid articleId);
        bool DeleteArticle(Article article);
        bool Save();
        string GetPublicIdFromImageUrl(string imageUrl);
        string GetPublicIdFromDocUrl(string docUrl);

    }
    public class AritcleService : IArticleService
    {
        private readonly ProjectDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Cloudinary _cloudinary;

        public AritcleService(ProjectDbContext context, IWebHostEnvironment webHostEnvironment, Cloudinary cloudinary, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _cloudinary = cloudinary;

        }
        public ICollection<Article> GetArticles()
        {
            return _context.Articles.OrderBy(p => p.Id).ToList();
        }
        public Article GetArticleByID(Guid id)
        {
            return _context.Articles.Where(p => p.Id == id).FirstOrDefault();
        }

        public async Task<IEnumerable<Article>> GetArticlesByTitle(string title)
        {
            return await _context.Articles.Where(p => p.Title.Contains(title)).ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetArticlesByFacultyId(int facultyID)
        {
            return await _context.Articles.Where(p => p.FacultyId == facultyID).ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetApprovedArticles(int facultyID)
        {
            return await _context.Articles
                .Where(p => p.PublishStatusId == (int)EPublishStatus.Approval 
                && p.FacultyId == facultyID)
                .ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetGuestApprovedArticles(int facultyID)
        {
            return await _context.Articles
                .Where(p => p.CoordinatorStatus == true 
                && p.PublishStatusId == (int)EPublishStatus.Approval
                && p.FacultyId == facultyID)
                .ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetArticleByCoordinatorStatusAndFacultyId(bool coordinatorStatus, int facultyId)
        {
            return await _context.Articles
                .Where(p => p.CoordinatorStatus == coordinatorStatus && p.FacultyId == facultyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticleByPublishStatusIdAndFacultyId(int publishStatusId, int facultyId)
        {
            return await _context.Articles
                .Where(p => p.FacultyId == facultyId && p.PublishStatusId == publishStatusId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetArticleByPublishStatus(int publishStatusId)
        {
            return await _context.Articles
                .Where(p => p.PublishStatusId == publishStatusId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetArticleByProfile(string userId)
        {
            return await _context.Articles
                .Where(p => p.StudentId == userId)
                .ToListAsync();
        }
        public bool ArticleExists(Guid articleId)
        {
            return _context.Articles.Any(p => p.Id == articleId);
        }
        public async Task<bool> UpdateArticle(Article article)
        {
            try
            {
                _context.Articles.Update(article);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> AddArticle(Article article)
        {
            try
            {
               
                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool DeleteArticle(Article article)
        {
            _context.Remove(article);

            return Save();
        }
        public bool IsValidImageFile(List<IFormFile> imageFiles)
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return false;
            }

            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".gif" }; // Add more file extensions as needed

            foreach (var imageFile in imageFiles)
            {
                // Get the file extension
                var fileExtension = Path.GetExtension(imageFile.FileName);

                // Check if the file extension is in the allowed extensions list
                if (!allowedExtensions.Contains(fileExtension?.ToLower()))
                {
                    return false;
                }

                // Perform additional checks if needed, such as content type validation.
            }

            return true;
        }

        public bool IsValidDocFile(IFormFile docFile)
        {
            if (docFile == null || docFile.Length == 0)
            {
                return true;
            }

            // Get the file extension
            var fileExtension = Path.GetExtension(docFile.FileName);

            // Define the allowed file extension for DOC files
            var allowedExtension = ".docx";

            // Check if the file extension matches the allowed extension
            if (!string.Equals(fileExtension, allowedExtension, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Perform additional checks if needed, such as content type validation.

            return true;
        }

        public string GetPublicIdFromImageUrl(string imageUrl)
        {
            // Example: https://res.cloudinary.com/{cloud_name}/image/upload/{public_id}.{format}
            // Extract public ID from the URL
            var segments = imageUrl.Split('/');
            var fileName = segments[segments.Length - 1]; // Get the last segment (filename)
            var publicId = fileName.Substring(0, fileName.LastIndexOf('.')); // Remove the file extension
            return publicId;
        }

        public string GetPublicIdFromDocUrl(string docUrl)
        {
            // Example: https://res.cloudinary.com/{cloud_name}/image/upload/{public_id}.{format}
            // Extract public ID from the URL
            var segments = docUrl.Split('/');
            var publicIdWithExtension = segments[segments.Length - 1]; // Get the last segment (filename with extension)
            var publicId = publicIdWithExtension.Substring(0, publicIdWithExtension.LastIndexOf('.')); // Remove the file extension
            return publicId;
        }

        public async Task<ImageUploadResult> UploadImage(ImageUploadParams parameters)
        {
            return await _cloudinary.UploadAsync(parameters);
        }

        public async Task<RawUploadResult> UploadFile(RawUploadParams parameters)
        {
            return await _cloudinary.UploadAsync(parameters);
        }
    }
}
