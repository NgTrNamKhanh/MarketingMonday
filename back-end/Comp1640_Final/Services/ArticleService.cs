﻿using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Hosting;
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
        Task<IEnumerable<Article>> GetStudentConfirmedArticle(int facultyID);
        Task<IEnumerable<Article>> GetStudentOnHoldArticle(int facultyID);
        Task<IEnumerable<Article>> GetStudentOnHoldNotCommentedArticle(int facultyID);
        Task<IEnumerable<Article>> GetStudentOnHoldCommentedArticle(int facultyID);
        Task<IEnumerable<Article>> GetStudentNotApprovaldArticle(int facultyID);
        Task<IEnumerable<Article>> GetArticleByPublishStatusIdAndFacultyId(int publishStatusId, int facultyId);
        bool IsValidImageFile(IFormFile imageFile);
        Task<string> SaveImageAsync(IFormFile imageFile);
        bool IsValidDocFile(IFormFile imageFile);
        Task<string> SaveDocAsync(IFormFile docFile);
        bool ArticleExists(Guid articleId);
        bool DeleteArticle(Article article);
        bool Save();


    }
    public class AritcleService : IAritcleService
    {
        private readonly ProjectDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AritcleService(ProjectDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IEnumerable<Article>> GetStudentConfirmedArticle(int facultyID)
        {
            return await _context.Articles
                .Where(p => p.FacultyId == facultyID && p.PublishStatusId == (int)EPublishStatus.Approval)
                .ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetStudentOnHoldArticle(int facultyID)
        {
            return await _context.Articles
                .Where(p => p.FacultyId == facultyID && p.PublishStatusId == (int)EPublishStatus.Pending)
                .ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetStudentOnHoldCommentedArticle(int facultyID)
        {
            return await _context.Articles
                .Where(p => p.FacultyId == facultyID && !string.IsNullOrWhiteSpace(p.CoordinatorComment) && p.PublishStatusId == (int)EPublishStatus.Pending)
                .ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetStudentOnHoldNotCommentedArticle(int facultyID)
        {
            return await _context.Articles
                .Where(p => p.FacultyId == facultyID && string.IsNullOrWhiteSpace(p.CoordinatorComment) && p.PublishStatusId == (int)EPublishStatus.Pending)
                .ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetStudentNotApprovaldArticle(int facultyID)
        {
            return await _context.Articles
                .Where(p => p.FacultyId == facultyID && p.PublishStatusId == (int)EPublishStatus.NotApproval)
                .ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetArticleByPublishStatusIdAndFacultyId(int publishStatusId, int facultyId)
        {
            return await _context.Articles
                .Where(p => p.FacultyId == facultyId && p.PublishStatusId == publishStatusId)
                .ToListAsync();
        }
        public bool ArticleExists(Guid articleId)
        {
            return _context.Articles.Any(p => p.Id == articleId);
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
        public bool IsValidImageFile(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return false;
            }

            // Get the file extension
            var fileExtension = Path.GetExtension(imageFile.FileName);

            // Define the allowed file extensions
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".gif" }; // Add more file extensions as needed

            // Check if the file extension is in the allowed extensions list
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                return false;
            }

            // Perform additional checks if needed, such as content type validation.

            return true;
        }
        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var imagesDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

            if (!Directory.Exists(imagesDirectory))
                Directory.CreateDirectory(imagesDirectory);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(imagesDirectory, fileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            return "\\Images\\" + fileName;
        }
        public bool IsValidDocFile(IFormFile docFile)
        {
            if (docFile == null || docFile.Length == 0)
            {
                return false;
            }

            // Get the file extension
            var fileExtension = Path.GetExtension(docFile.FileName);

            // Define the allowed file extension for DOC files
            var allowedExtension = ".doc";

            // Check if the file extension matches the allowed extension
            if (!string.Equals(fileExtension, allowedExtension, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Perform additional checks if needed, such as content type validation.

            return true;
        }
        public async Task<string> SaveDocAsync(IFormFile docFile)
        {
            var docsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Docs");

            if (!Directory.Exists(docsDirectory))
                Directory.CreateDirectory(docsDirectory);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(docFile.FileName);
            var filePath = Path.Combine(docsDirectory, fileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await docFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            return "\\Docs\\" + fileName;
        }

    }
}
