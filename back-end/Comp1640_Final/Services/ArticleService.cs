﻿using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Comp1640_Final.Services
{
    public interface IAritcleService
    {
        ICollection<Article> GetArticles();
        Article GetArticleByID(Guid id);
        Task<IEnumerable<Article>> GetArticlesByTitle(string title);
        Task<IEnumerable<Article>> GetArticlesByFacultyId(int facultyID);
        Task<IEnumerable<Article>> GetArticleByPublishStatusIdAndFacultyId(int publishStatusId, int facultyId);
        Task<IEnumerable<Article>> GetApprovedArticles(int facultyID);
        Task<IEnumerable<Article>> GetArticleByCoordinatorStatusAndFacultyId(bool coordinatorStatus, int facultyId);
        Task<IEnumerable<Article>> GetArticleByProfile(string userId);
        Task<IEnumerable<Article>> GetArticleByPublishStatus(int publishStatusId);
        Task<bool> UpdateArticle(Article article);
        Task<bool> AddArticle(Article article);
        bool IsValidImageFile(List<IFormFile> imageFile);
        Task<IEnumerable<string>> SaveImages(List<IFormFile> imageFile, string subFolderName);
        bool IsValidDocFile(IFormFile imageFile);
        Task<string> SaveDoc(IFormFile docFile, string subFolderName);
        bool ArticleExists(Guid articleId);
        bool DeleteArticle(Article article);
        Task<IEnumerable<byte[]>> GetImagesByArticleId(Guid articleId);
        bool Save();
        string GetPublicIdFromImageUrl(string imageUrl);
        string GetPublicIdFromDocUrl(string docUrl);

    }
    public class AritcleService : IAritcleService
    {
        private readonly ProjectDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public AritcleService(ProjectDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
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
                .Where(p => p.CoordinatorStatus == true && p.PublishStatusId == (int)EPublishStatus.Approval
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
        public async Task<IEnumerable<string>> SaveImages(List<IFormFile> imageFiles, string subfolderName)
        {
            var imagesDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Images", subfolderName);

            if (!Directory.Exists(imagesDirectory))
                Directory.CreateDirectory(imagesDirectory);

            var imagePaths = new List<string>();

            foreach (var imageFile in imageFiles)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(imagesDirectory, fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }

                imagePaths.Add("\\Images\\" + subfolderName + "\\" + fileName);
            }

            return imagePaths;
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
        public async Task<string> SaveDoc(IFormFile docFile, string subfolderName)
        {
            var docsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Docs", subfolderName);

            if (!Directory.Exists(docsDirectory))
                Directory.CreateDirectory(docsDirectory);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(docFile.FileName);
            var filePath = Path.Combine(docsDirectory, fileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await docFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            return "\\Docs\\" + subfolderName + "\\" + fileName;
        }

        public async Task<IEnumerable<byte[]>> GetImagesByArticleId(Guid articleId)
        {
            var article = await _context.Articles.FindAsync(articleId);

            if (article == null)
            {
                return null; // Or handle as needed
            }

            var imagePaths = article.ImagePath?.Split(';');
            var imageBytesList = new List<byte[]>();

            foreach (var imagePath in imagePaths)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('\\'));

                if (System.IO.File.Exists(filePath))
                {
                    var imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    imageBytesList.Add(imageBytes);
                }
                else
                {
                    // Optionally log or handle the case where the image file is not found
                }
            }

            return imageBytesList;
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
    }
}
