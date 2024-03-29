﻿using AutoMapper;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Comp1640_Final.Data;
using Comp1640_Final.DTO.Request;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Comp1640_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IAritcleService _articleService;
        private readonly IMapper _mapper;
        private readonly ProjectDbContext _context;
        private static IWebHostEnvironment _webHostEnvironment;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;
        private readonly IDislikeService _dislikeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Cloudinary _cloudinary;

        public ArticlesController(IAritcleService articleService,
            IMapper mapper,
            ProjectDbContext context,
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            ICommentService commentService,
            ILikeService likeService,
            IDislikeService dislikeService,
            Cloudinary cloudinary,
            IHttpContextAccessor httpContextAccessor)
        {
            _articleService = articleService;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _userService = userService;
            _commentService = commentService;
            _likeService = likeService;
            _dislikeService = dislikeService;
            _cloudinary = cloudinary;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> GetArticles(string userId)
        {
            var articles = _articleService.GetArticles();
            var articleDTOs = new List<SubmissionResponse>();

            foreach (var article in articles)
            {
                var user = await _userManager.FindByIdAsync(article.StudentId);
                var articleDTO = _mapper.Map<SubmissionResponse>(article);
                //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
                var imageBytes = await _articleService.GetImagesByArticleId(article.Id);

                var userImageBytes = await _userService.GetImagesByUserId(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (userImageBytes == null)
                {
                    var defaultImageFileName = "default-avatar.jpg";
                    var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                    userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                }
                articleDTO.StudentAvatar = userImageBytes;
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTOs.Add(articleDTO);
            }

            return Ok(articleDTOs);
        }
        [HttpGet("id/{articleId}")]
        public async Task<IActionResult> GetArticleByID(Guid articleId, string userId)
        {
            if (!_articleService.ArticleExists(articleId))
            {
                return NotFound();

            }
            var article = _articleService.GetArticleByID(articleId);
            var articleDTO = _mapper.Map<SubmissionResponse>(article);
            var user = await _userManager.FindByIdAsync(article.StudentId);
            var imageBytes = await _articleService.GetImagesByArticleId(articleId);
            var userImageBytes = await _userService.GetImagesByUserId(user.Id); // Await the method call

            // If imageBytes is null, read the default image file
            if (userImageBytes == null)
            {
                var defaultImageFileName = "default-avatar.jpg";
                var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
            }
            articleDTO.StudentAvatar = userImageBytes;
            //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
            articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
            articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
            articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
            articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
            articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
            articleDTO.StudentName = user.FirstName + " " + user.LastName;
            articleDTO.ImageBytes = imageBytes.ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(articleDTO);
        }

        [HttpGet("title/{articleTitle}")]
        public async Task<IActionResult> GetArticleByTitle(string articleTitle, string userId)
        {
            var articles = await _articleService.GetArticlesByTitle(articleTitle);

            if (articles == null || !articles.Any())
                return NotFound(); var articleDTOs = new List<SubmissionResponse>();

            foreach (var article in articles)
            {
                var user = await _userManager.FindByIdAsync(article.StudentId);
                var articleDTO = _mapper.Map<SubmissionResponse>(article);
                //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
                var imageBytes = await _articleService.GetImagesByArticleId(article.Id);
                var userImageBytes = await _userService.GetImagesByUserId(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (userImageBytes == null)
                {
                    var defaultImageFileName = "default-avatar.jpg";
                    var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                    userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                }
                articleDTO.StudentAvatar = userImageBytes;
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTOs.Add(articleDTO);
            }

            return Ok(articleDTOs);
        }

        [HttpGet("faculty/{facultyId}")]
        public async Task<IActionResult> GetArticleByFacultyId(int facultyId, string userId)
        {
            var articles = await _articleService.GetArticlesByFacultyId(facultyId);

            if (articles == null || !articles.Any())
                return BadRequest("There is no submission here");

            var articleDTOs = new List<SubmissionResponse>();

            foreach (var article in articles)
            {
                var imageBytes = await _articleService.GetImagesByArticleId(article.Id);
                var user = await _userManager.FindByIdAsync(article.StudentId);
                var articleDTO = _mapper.Map<SubmissionResponse>(article);
                var userImageBytes = await _userService.GetImagesByUserId(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (userImageBytes == null)
                {
                    var defaultImageFileName = "default-avatar.jpg";
                    var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                    userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                }
                articleDTO.StudentAvatar = userImageBytes;
                //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTOs.Add(articleDTO);
            }
            return Ok(articleDTOs);
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetArticleByProfile(string userId)
        {
            var articles = await _articleService.GetArticleByProfile(userId);

            if (articles == null || !articles.Any())
                return BadRequest("There is no submission here");

            var articleDTOs = new List<SubmissionResponse>();

            foreach (var article in articles)
            {
                var imageBytes = await _articleService.GetImagesByArticleId(article.Id);
                var user = await _userManager.FindByIdAsync(article.StudentId);
                var articleDTO = _mapper.Map<SubmissionResponse>(article);
                var userImageBytes = await _userService.GetImagesByUserId(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (userImageBytes == null)
                {
                    var defaultImageFileName = "default-avatar.jpg";
                    var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                    userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                }
                articleDTO.StudentAvatar = userImageBytes;
                //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTOs.Add(articleDTO);
            }
            return Ok(articleDTOs);
        }
        [HttpGet("approved/faculty")]
        public async Task<IActionResult> GetApprovedAticles(int facultyId, string userId)
        {
            var articles = await _articleService.GetApprovedArticles(facultyId);

            if (articles == null || !articles.Any())
                return BadRequest("There is no article here");

            var articleDTOs = new List<ArticleResponse>();

            foreach (var article in articles)
            {
                var user = await _userManager.FindByIdAsync(article.StudentId);
                var articleDTO = _mapper.Map<ArticleResponse>(article);
                //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
                var imageBytes = await _articleService.GetImagesByArticleId(article.Id);
                var userImageBytes = await _userService.GetImagesByUserId(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (userImageBytes == null)
                {
                    var defaultImageFileName = "default-avatar.jpg";
                    var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                    userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                }
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.StudentAvatar = userImageBytes;
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTOs.Add(articleDTO);
            }
            return Ok(articleDTOs);
        }
        [HttpGet("status/{publishStatusId}/faculty/{facultyId}")]
        public async Task<IActionResult> GetArticleByPublishStatusIdAndFacultyId(int publishStatusId, int facultyId, string userId)
        {
            var articles = await _articleService.GetArticleByPublishStatusIdAndFacultyId(publishStatusId, facultyId);

            if (articles == null || !articles.Any())
                return BadRequest("There is no submission here");

            var articleDTOs = new List<SubmissionResponse>();

            foreach (var article in articles)
            {
                var user = await _userManager.FindByIdAsync(article.StudentId);
                var articleDTO = _mapper.Map<SubmissionResponse>(article);
                //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
                var imageBytes = await _articleService.GetImagesByArticleId(article.Id);
                var userImageBytes = await _userService.GetImagesByUserId(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (userImageBytes == null)
                {
                    var defaultImageFileName = "default-avatar.jpg";
                    var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                    userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                }
                articleDTO.StudentAvatar = userImageBytes;
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTOs.Add(articleDTO);
            }

            return Ok(articleDTOs);
        }

        [HttpGet("publishStatus/{publishStatusId}")]
        public async Task<IActionResult> GetArticleByPublishStatus(int publishStatusId, string userId)
        {
            var articles = await _articleService.GetArticleByPublishStatus(publishStatusId);

            if (articles == null || !articles.Any())
                return NotFound();

            var articleDTOs = new List<SubmissionResponse>();

            foreach (var article in articles)
            {
                var articleDTO = _mapper.Map<SubmissionResponse>(article);
                var user = await _userManager.FindByIdAsync(article.StudentId);

                //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
                var imageBytes = await _articleService.GetImagesByArticleId(article.Id);
                var userImageBytes = await _userService.GetImagesByUserId(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (userImageBytes == null)
                {
                    var defaultImageFileName = "default-avatar.jpg";
                    var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                    userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                }
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.StudentAvatar = userImageBytes;
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTOs.Add(articleDTO);
            }

            return Ok(articleDTOs);
        }

        [HttpGet("GetImages/{articleId}")]
        public async Task<IActionResult> GetImagesByArticleId(Guid articleId)
        {
            var imageBytesList = await _articleService.GetImagesByArticleId(articleId);

            if (imageBytesList == null || !imageBytesList.Any())
            {
                return NotFound("No images found for the article");
            }

            return Ok(imageBytesList);
        }

        [HttpGet("DownloadDocument/{articleId}")]
        public async Task<IActionResult> DownloadDocument(Guid articleId)
        {
            var article = _articleService.GetArticleByID(articleId);

            if (article == null)
            {
                return NotFound("Article not found");
            }

            // Get the file path of the document associated with the article
            var documentPath = article.DocPath;

            if (string.IsNullOrEmpty(documentPath))
            {
                return NotFound("Document not found for the article");
            }

            // Combine the file path with the web root path to get the absolute file path
            var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, documentPath.TrimStart('\\'));

            // Check if the file exists
            if (!System.IO.File.Exists(absolutePath))
            {
                return NotFound("Document file not found");
            }

            // Return the file for download
            var fileStream = System.IO.File.OpenRead(absolutePath);
            return File(fileStream, "application/octet-stream", Path.GetFileName(absolutePath));
        }

        [HttpPost("student")]
        public async Task<ActionResult<Article>> AddArticle([FromForm] AddArticleDTO articleAdd)
        {
            if (articleAdd == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var articleMap = _mapper.Map<Article>(articleAdd);
            articleMap.Id = Guid.NewGuid();
            articleMap.PublishStatusId = (int)EPublishStatus.Pending;
            articleMap.UploadDate = DateTime.Now;

            if (articleAdd.ImageFiles.Count > 0)
            {
                try
                {
                    if (!_articleService.IsValidImageFile(articleAdd.ImageFiles))
                    {
                        return BadRequest("Invalid image file format. Only PNG, JPG, JPEG, and GIF are allowed.");
                    }

                    var imagePaths = await _articleService.SaveImages(articleAdd.ImageFiles, articleMap.Id.ToString());
                    if (imagePaths.Any())
                    {
                        articleMap.ImagePath = string.Join(";", imagePaths);
                    }
                    else
                    {
                        return BadRequest("Failed to save image files.");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }

            if (articleAdd.DocFiles.Length > 0)
            {
                try
                {
                    if (!_articleService.IsValidDocFile(articleAdd.DocFiles))
                    {
                        return BadRequest("Invalid doc file format. Only DOC are allowed.");
                    }

                    var docPath = await _articleService.SaveDoc(articleAdd.DocFiles, articleMap.Id.ToString());
                    articleMap.DocPath = docPath;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }

            if (articleAdd.ImageFiles.Count > 0)
            {
                try
                {
                    var uploadResults = new List<ImageUploadResult>();
                    foreach (var file in articleAdd.ImageFiles)
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, file.OpenReadStream())
                        };
                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        uploadResults.Add(uploadResult);
                    }

                    var imageUrls = uploadResults.Select(r => r.Uri.ToString()).ToList();
                    articleMap.CloudImagePath = string.Join(";", imageUrls);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            if (!await _articleService.AddArticle(articleMap))
            {
                return BadRequest("Failed to add article.");
            }

            return Ok("Successfully added");
        }


        [HttpPut("{articleId}")]
        public async Task<ActionResult<Article>> UpdateArticle([FromForm] EditArticleDTO articleUpdate)
        {
            if (articleUpdate == null)
                return BadRequest(ModelState);

            if (!_articleService.ArticleExists(articleUpdate.Id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var articleMap = _mapper.Map<Article>(articleUpdate);
            articleMap.UploadDate = DateTime.Now;
            var article = _articleService.GetArticleByID(articleMap.Id);

            if (articleUpdate.ImageFiles.Count > 0)
            {
                try
                {
                    if (!_articleService.IsValidImageFile(articleUpdate.ImageFiles))
                    {
                        return BadRequest("Invalid image file format. Only PNG, JPG, JPEG, and GIF are allowed.");
                    }

                    var imagePaths = await _articleService.SaveImages(articleUpdate.ImageFiles, articleMap.Id.ToString());

                    // Delete old image files
                    var oldImagePaths = article.ImagePath?.Split(';');
                    foreach (var oldImagePath in oldImagePaths)
                    {
                        var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, oldImagePath.TrimStart('\\'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    articleMap.ImagePath = string.Join(";", imagePaths);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }

            if (articleUpdate.DocFiles.Length > 0)
            {
                try
                {
                    if (!_articleService.IsValidDocFile(articleUpdate.DocFiles))
                    {
                        return BadRequest("Invalid doc file format. Only DOC are allowed.");
                    }

                    var docPath = await _articleService.SaveDoc(articleUpdate.DocFiles, articleMap.Id.ToString());

                    // Delete old document file
                    var oldDocPath = Path.Combine(_webHostEnvironment.WebRootPath, article.DocPath.TrimStart('\\'));
                    if (System.IO.File.Exists(oldDocPath))
                    {
                        System.IO.File.Delete(oldDocPath);
                    }

                    articleMap.DocPath = docPath;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }

            if (articleUpdate.ImageFiles.Count > 0)
            {
                try
                {
                    // Delete old images from Cloudinary
                    if (!string.IsNullOrEmpty(article.CloudImagePath))
                    {
                        var oldImagePaths = article.CloudImagePath.Split(';');
                        foreach (var oldImagePath in oldImagePaths)
                        {
                            // Delete old image from Cloudinary
                            var publicId = _articleService.GetPublicIdFromImageUrl(oldImagePath);
                            await _cloudinary.DeleteResourcesAsync(publicId);
                        }
                    }
                    // Save new images to Cloudinary
                    var uploadResults = new List<ImageUploadResult>();
                    foreach (var file in articleUpdate.ImageFiles)
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, file.OpenReadStream())
                        };
                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        uploadResults.Add(uploadResult);
                    }

                    // Update article with new image URLs
                    var imageUrls = uploadResults.Select(r => r.Uri.ToString()).ToList();
                    articleMap.CloudImagePath = string.Join(";", imageUrls);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }

            if (article != null)
            {
                _context.Entry(article).State = EntityState.Detached;
            }

            if (!await _articleService.UpdateArticle(articleMap))
            {
                return BadRequest("Failed to update article.");
            }

            return Ok("Successfully updated");
        }

        [HttpPut("updateStatus/{articleId}")]
        public async Task<ActionResult<Article>> UpdateStatusArticle(Guid articleId, int publicStatus)
        {
            if (publicStatus < 0 || publicStatus > 3 || publicStatus == null)
                return BadRequest(ModelState);

            if (!_articleService.ArticleExists(articleId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var article = _articleService.GetArticleByID(articleId);

            article.Id = articleId;
            article.PublishStatusId = publicStatus;

            if (!await _articleService.UpdateArticle(article))
            {
                return BadRequest("Failed to update article.");
            }

            return Ok("Successfully change article status");
        }

        [HttpPut("addComment/{articleId}")]
        public async Task<ActionResult<Article>> AddCommentArticle(Guid articleId, string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
                return BadRequest(ModelState);

            if (!_articleService.ArticleExists(articleId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var article = _articleService.GetArticleByID(articleId);

            article.Id = articleId;
            article.CoordinatorComment = comment;
            var coordinator = _context.Users
                   .Where(u => u.FacultyId == article.FacultyId)
                   .Join(_context.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRole = ur })
                   .Join(_context.Roles, ur => ur.UserRole.RoleId, r => r.Id, (ur, r) => new { User = ur.User, Role = r })
                   .FirstOrDefault(ur => ur.Role.Name == "Coordinator")
                   ?.User;
            article.MarketingCoordinatorId = coordinator.Id;
            if (!await _articleService.UpdateArticle(article))
            {
                return BadRequest("Failed to update article.");
            }

            return Ok("Successfully add commnet for article");
        }

        [HttpDelete("{articleId}")]
        public async Task<IActionResult> DeleteArticle(Guid articleId)
        {
            if (!_articleService.ArticleExists(articleId))
            {
                return NotFound();
            }
            var articleToDelete = _articleService.GetArticleByID(articleId);

            var imagesDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Images", articleId.ToString());
            if (Directory.Exists(imagesDirectory))
            {
                Directory.Delete(imagesDirectory, true);
            }

            var docsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Docs", articleId.ToString());
            if (Directory.Exists(docsDirectory))
            {
                Directory.Delete(docsDirectory, true);
            }

            if (!string.IsNullOrEmpty(articleToDelete.CloudImagePath))
            {
                try
                {
                    var imageUrls = articleToDelete.CloudImagePath.Split(';');
                    foreach (var imageUrl in imageUrls)
                    {
                        // Extract public ID from Cloudinary image URL
                        var publicId = _articleService.GetPublicIdFromImageUrl(imageUrl);
                        // Delete image from Cloudinary
                        await _cloudinary.DeleteResourcesAsync(publicId);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            if (!_articleService.DeleteArticle(articleToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting article");
            }

            return Ok("Successfully delete article");
        }
        //private async Task<string> GetUserId()
        //{
        //    var principal = _httpContextAccessor.HttpContext.User;
        //    var user = await _userManager.FindByEmailAsync(principal.Identity.Name);
        //    if (user != null)
        //    {
        //        return user.Id;
        //    }
        //    return null;
        //}


    }
}