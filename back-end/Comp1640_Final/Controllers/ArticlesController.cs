using AutoMapper;
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
using Comp1640_Final.Migrations;
using static System.Net.Mime.MediaTypeNames;
using System.IO.Compression;
using System.Reflection.Metadata;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Document = DocumentFormat.OpenXml.Wordprocessing.Document;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
namespace Comp1640_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
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

        public ArticlesController(IArticleService articleService,
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

                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                articleDTO.StudentAvatar = cloudUserImage;
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.ViewCount = article.ViewCount;
                articleDTO.IsViewed = article.ViewCount >= 1;
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
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
            var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

            // If imageBytes is null, read the default image file
            if (cloudUserImage == null)
            {
                var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                cloudUserImage = defaultImageFileName;
            }
            articleDTO.StudentAvatar = cloudUserImage;
            //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
            articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
            articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
            articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
            articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
            articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
            articleDTO.ViewCount = article.ViewCount;
            articleDTO.IsViewed = article.ViewCount >= 1;
            articleDTO.StudentName = user.FirstName + " " + user.LastName;
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
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                articleDTO.StudentAvatar = cloudUserImage;
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.ViewCount = article.ViewCount;
                articleDTO.IsViewed = article.ViewCount >= 1;
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
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
                var user = await _userManager.FindByIdAsync(article.StudentId);
                var articleDTO = _mapper.Map<SubmissionResponse>(article);
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                articleDTO.StudentAvatar = cloudUserImage;
                //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.ViewCount = article.ViewCount;
                articleDTO.IsViewed = article.ViewCount >= 1;
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
                var user = await _userManager.FindByIdAsync(article.StudentId);
                var articleDTO = _mapper.Map<SubmissionResponse>(article);
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                articleDTO.StudentAvatar = cloudUserImage;
                //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.ViewCount = article.ViewCount;
                articleDTO.IsViewed = article.ViewCount >= 1;
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
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                articleDTO.StudentAvatar = cloudUserImage;
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.ViewCount = article.ViewCount;
                articleDTO.IsViewed = article.ViewCount >= 1;
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTOs.Add(articleDTO);
            }
            return Ok(articleDTOs);
        }

        [HttpGet("guest/faculty")]
        public async Task<IActionResult> GetGuestApprovedAticles(int facultyId, string userId)
        {
            var articles = await _articleService.GetGuestApprovedArticles(facultyId);

            if (articles == null || !articles.Any())
                return BadRequest("There is no article here");

            var articleDTOs = new List<ArticleResponse>();

            foreach (var article in articles)
            {
                var user = await _userManager.FindByIdAsync(article.StudentId);
                var articleDTO = _mapper.Map<ArticleResponse>(article);
                //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                articleDTO.StudentAvatar = cloudUserImage;
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.ViewCount = article.ViewCount;
                articleDTO.IsViewed = article.ViewCount >= 1;
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTOs.Add(articleDTO);
            }
            return Ok(articleDTOs);
        }

        [HttpGet("status/{publishStatusId}/faculty/{facultyId}")]
        public async Task<IActionResult> GetArticleByPublishStatusIdAndFacultyId(int publishStatusId, int facultyId, string userId)
        {
            if (publishStatusId < 0 || publishStatusId > 3 || publishStatusId == null)
                return BadRequest("Error publish status input");

            var articles = await _articleService.GetArticleByPublishStatusIdAndFacultyId(publishStatusId, facultyId);

            if (articles == null || !articles.Any())
                return BadRequest("There is no submission here");

            var articleDTOs = new List<SubmissionResponse>();

            foreach (var article in articles)
            {
                var user = await _userManager.FindByIdAsync(article.StudentId);
                var articleDTO = _mapper.Map<SubmissionResponse>(article);
                //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                articleDTO.StudentAvatar = cloudUserImage;
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.ViewCount = article.ViewCount;
                articleDTO.IsViewed = article.ViewCount >= 1;
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTOs.Add(articleDTO);
            }

            return Ok(articleDTOs);
        }

        [HttpGet("coordinatorStatus/{CoordinatorStatus}/faculty/{facultyId}")]
        public async Task<IActionResult> GetArticleByCoordinatorStatusAndFacultyId(bool CoordinatorStatus, int facultyId, string userId)
        {
            var articles = await _articleService.GetArticleByCoordinatorStatusAndFacultyId(CoordinatorStatus, facultyId);

            if (articles == null || !articles.Any())
                return BadRequest("There is no submission here");

            var articleDTOs = new List<SubmissionResponse>();

            foreach (var article in articles)
            {
                var user = await _userManager.FindByIdAsync(article.StudentId);
                var articleDTO = _mapper.Map<SubmissionResponse>(article);
                //articleDTO.UploadDate = article.UploadDate.ToString("dd/MM/yyyy");
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                articleDTO.StudentAvatar = cloudUserImage;
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.ViewCount = article.ViewCount;
                articleDTO.IsViewed = article.ViewCount >= 1;
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
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
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                articleDTO.StudentAvatar = cloudUserImage;
                articleDTO.CommmentCount = await _commentService.GetCommentsCount(article.Id);
                articleDTO.LikeCount = await _likeService.GetArticleLikesCount(article.Id);
                articleDTO.DislikeCount = await _dislikeService.GetArticleDislikesCount(article.Id);
                articleDTO.IsLiked = await _likeService.IsArticleLiked(userId, article.Id);
                articleDTO.IsDisliked = await _dislikeService.IsArticleDisLiked(userId, article.Id);
                articleDTO.ViewCount = article.ViewCount;
                articleDTO.IsViewed = article.ViewCount >= 1;
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTOs.Add(articleDTO);
            }

            return Ok(articleDTOs);
        }

        [HttpGet("DownloadDocument/{articleId}")]
        public async Task<IActionResult> DownloadDocument(Guid articleId)
        {
            var article = _articleService.GetArticleByID(articleId);

            if (article == null)
            {
                return NotFound("Article not found");
            }

            // Get the Cloudinary URL of the document associated with the article
            var documentUrl = article.CloudDocPath;

            if (string.IsNullOrEmpty(documentUrl))
            {
                return NotFound("Document not found for the article");
            }

            try
            {
                // Generate a secure URL for downloading the document from Cloudinary
                var secureUrl = _cloudinary.Api.UrlImgUp
                    .Secure()
                    .Transform(new Transformation().FetchFormat("auto")) // Optional: Apply transformations if needed
                    .BuildUrl(documentUrl);

                // Redirect the user to the secure URL for downloading the document
                return Redirect(secureUrl);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to generate download URL for the document: {ex.Message}");
            }
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

                    var uploadResults = new List<ImageUploadResult>();
                    foreach (var file in articleAdd.ImageFiles)
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, file.OpenReadStream())
                        };
                        var uploadResult = await _articleService.UploadImage(uploadParams);
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

            if (articleAdd.DocFiles.Length > 0)
            {
                try
                {
                    if (!_articleService.IsValidDocFile(articleAdd.DocFiles))
                    {
                        return BadRequest("Invalid doc file format. Only DOC are allowed.");
                    }

                    // Save new files to Cloudinary
                    var uploadParams = new RawUploadParams
                    {
                        File = new FileDescription(articleAdd.DocFiles.FileName, articleAdd.DocFiles.OpenReadStream())
                    };
                    var uploadResult = await _articleService.UploadFile(uploadParams);

                    // Update article with new file URLs
                    articleMap.CloudDocPath = uploadResult.Url.ToString();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
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
            if (article.PublishStatusId == (int)EPublishStatus.Approval)
            {
                return BadRequest("Approved article cannot be updated");
            }

            try
            {
                if (articleUpdate.ImageFiles.Count > 0)
                {
                    var newImageUrls = await _articleService.UploadImages(articleUpdate.ImageFiles);
                    await _articleService.DeleteImagesFromCloudinary(article.CloudImagePath);
                    articleMap.CloudImagePath = string.Join(";", newImageUrls);
                }

                if (articleUpdate.DocFiles.Length > 0)
                {
                    var newDocUrl = await _articleService.UploadDocument(articleUpdate.DocFiles);
                    await _articleService.DeleteDocumentFromCloudinary(article.CloudDocPath);
                    articleMap.CloudDocPath = newDocUrl;
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPut("{articleId}")]
        //public async Task<ActionResult<Article>> UpdateArticle1([FromForm] EditArticleDTO articleUpdate)
        //{
        //    if (articleUpdate == null)
        //        return BadRequest(ModelState);

        //    if (!_articleService.ArticleExists(articleUpdate.Id))
        //        return NotFound();

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var articleMap = _mapper.Map<Article>(articleUpdate);
        //    articleMap.UploadDate = DateTime.Now;
        //    var article = _articleService.GetArticleByID(articleMap.Id);
        //    if (article.PublishStatusId == (int)EPublishStatus.Approval)
        //    {
        //        return BadRequest("Approved article can not be update");
        //    }

        //    if (articleUpdate.ImageFiles.Count > 0)
        //    {
        //        try
        //        {
        //            if (!_articleService.IsValidImageFile(articleUpdate.ImageFiles))
        //            {
        //                return BadRequest("Invalid image file format. Only PNG, JPG, JPEG, and GIF are allowed.");
        //            }

        //            // Delete old images from Cloudinary
        //            if (!string.IsNullOrEmpty(article.CloudImagePath))
        //            {
        //                var oldImagePaths = article.CloudImagePath.Split(';');
        //                foreach (var oldImagePath in oldImagePaths)
        //                {
        //                    // Delete old image from Cloudinary
        //                    var publicId = _articleService.GetPublicIdFromImageUrl(oldImagePath);
        //                    await _cloudinary.DeleteResourcesAsync(publicId);
        //                }
        //            }
        //            // Save new images to Cloudinary
        //            var uploadResults = new List<ImageUploadResult>();
        //            foreach (var file in articleUpdate.ImageFiles)
        //            {
        //                var uploadParams = new ImageUploadParams
        //                {
        //                    File = new FileDescription(file.FileName, file.OpenReadStream())
        //                };
        //                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        //                uploadResults.Add(uploadResult);
        //            }

        //            // Update article with new image URLs
        //            var imageUrls = uploadResults.Select(r => r.Uri.ToString()).ToList();
        //            articleMap.CloudImagePath = string.Join(";", imageUrls);
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.ToString());
        //        }
        //    }

        //    if (articleUpdate.DocFiles.Length > 0)
        //    {
        //        try
        //        {
        //            if (!_articleService.IsValidDocFile(articleUpdate.DocFiles))
        //            {
        //                return BadRequest("Invalid doc file format. Only DOC are allowed.");
        //            }

        //            // Delete old images from Cloudinary
        //            if (!string.IsNullOrEmpty(article.CloudDocPath))
        //            {
        //                var publicDocId = _articleService.GetPublicIdFromDocUrl(article.CloudDocPath);
        //                await _cloudinary.DeleteResourcesAsync(publicDocId);
        //            }

        //            // Save new files to Cloudinary
        //            var uploadParams = new RawUploadParams
        //            {
        //                File = new FileDescription(articleUpdate.DocFiles.FileName, articleUpdate.DocFiles.OpenReadStream())
        //            };
        //            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        //            // Update article with new file URLs
        //            articleMap.CloudDocPath = uploadResult.Url.ToString();
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.ToString());
        //        }
        //    }

        //    if (article != null)
        //    {
        //        _context.Entry(article).State = EntityState.Detached;
        //    }

        //    if (!await _articleService.UpdateArticle(articleMap))
        //    {
        //        return BadRequest("Failed to update article.");
        //    }

        //    return Ok("Successfully updated");
        //}

        [HttpPut("updatePublishStatus/{articleId}")]
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

        [HttpPut("updateCoordinatorStatus/{articleId}")]
        public async Task<ActionResult<Article>> UpdateCoordinatorStatusArticle(Guid articleId)
        {
            if (!_articleService.ArticleExists(articleId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var article = _articleService.GetArticleByID(articleId);

            article.Id = articleId;

            article.CoordinatorStatus = !article.CoordinatorStatus;

            if (!await _articleService.UpdateArticle(article))
            {
                return BadRequest("Failed to update article coordinator status.");
            }

            return Ok("Successfully change article status");
        }

        [HttpPut("updateListCoordinatorStatus")]
        public async Task<ActionResult> UpdateListCoordinatorStatusArticle([FromForm] List<Guid> articleIds)
        {
            if (articleIds == null || articleIds.Count == 0)
                return BadRequest("No article IDs provided.");

            foreach (var articleId in articleIds)
            {
                if (!_articleService.ArticleExists(articleId))
                    return NotFound($"Article with ID {articleId} not found.");

                var article = _articleService.GetArticleByID(articleId);

                // Toggle the coordinator status for each article
                article.CoordinatorStatus = true;

                if (!await _articleService.UpdateArticle(article))
                {
                    return BadRequest($"Failed to update coordinator status for article with ID {articleId}.");
                }
            }

            return Ok("Successfully changed coordinator status for all articles.");
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

            if (!string.IsNullOrEmpty(articleToDelete.CloudImagePath))
            {
                try
                {
                    await _articleService.DeleteImagesFromCloudinary(articleToDelete.CloudImagePath);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            if (!string.IsNullOrEmpty(articleToDelete.CloudDocPath))
            {
                try
                {
                    await _articleService.DeleteDocumentFromCloudinary(articleToDelete.CloudDocPath);
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
        [HttpPost("download")]
		public async Task<IActionResult> DownloadSubmission([FromBody] DownloadArticleDTO downloadArticle)
		{
			// Create a memory stream to hold the generated Word document
			using (MemoryStream stream = new MemoryStream())
			{
				// Create a WordprocessingDocument
				using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
				{
					// Add a main document part
					MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

					// Create the Document tree
					mainPart.Document = new Document();
					Body body = mainPart.Document.AppendChild(new Body());

					// Add student name to the Word document
					Paragraph studentNameParagraph = body.AppendChild(new Paragraph());
					Run studentNameRun = studentNameParagraph.AppendChild(new Run());
					studentNameRun.AppendChild(new Text(downloadArticle.StudentName));

					// Add content to the Word document
					Paragraph titleParagraph = body.AppendChild(new Paragraph());
					Run titleRun = titleParagraph.AppendChild(new Run());
					titleRun.AppendChild(new Text(downloadArticle.Title));

					Paragraph descriptionParagraph = body.AppendChild(new Paragraph());
					Run descriptionRun = descriptionParagraph.AppendChild(new Run());
					descriptionRun.AppendChild(new Text(downloadArticle.Description));

					// Add images to the Word document
					foreach (var imageBytes in downloadArticle.ImageFiles)
					{
						ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
						using (MemoryStream imageStream = new MemoryStream(imageBytes))
						{
							imagePart.FeedData(imageStream);
						}

						AddImageToBody(mainPart.GetIdOfPart(imagePart), body);
					}

					// Save changes to the Word document
					wordDocument.Save();
				}

				// Reset the stream position to start
				stream.Position = 0;

				// Create a memory stream for the ZIP file
				using (MemoryStream zipStream = new MemoryStream())
				{
					// Create a ZIP archive
					using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
					{
						// Add the Word document to the ZIP archive
						ZipArchiveEntry entry = zipArchive.CreateEntry("submission.docx");
						using (Stream entryStream = entry.Open())
						{
							stream.CopyTo(entryStream);
						}

						// Add other files to the ZIP archive if needed
						// For example, if you have additional files to include, you can add them here
						// zipArchive.CreateEntry("filename.txt").Open().Write(...);
					}

					// Reset the stream position to start
					zipStream.Position = 0;

					// Return the ZIP file as a file download
					return File(zipStream.ToArray(), "application/zip", "submission.zip");
				}
			}
		}

		private void AddImageToBody(string relationshipId, Body body)
		{
			// Define the reference of the image.
			var element =
				new Drawing(
					new DW.Inline(
						new DW.Extent() { Cx = 990000L, Cy = 792000L },
						new DW.EffectExtent()
						{
							LeftEdge = 0L,
							TopEdge = 0L,
							RightEdge = 0L,
							BottomEdge = 0L
						},
						new DW.DocProperties()
						{
							Id = (UInt32Value)1U,
							Name = "Picture 1"
						},
						new DW.NonVisualGraphicFrameDrawingProperties(
							new A.GraphicFrameLocks() { NoChangeAspect = true }),
						new A.Graphic(
							new A.GraphicData(
								new PIC.Picture(
									new PIC.NonVisualPictureProperties(
										new PIC.NonVisualDrawingProperties()
										{
											Id = (UInt32Value)0U,
											Name = "New Bitmap Image.jpg"
										},
										new PIC.NonVisualPictureDrawingProperties()),
									new PIC.BlipFill(
										new A.Blip(
											new A.BlipExtensionList(
												new A.BlipExtension()
												{
													Uri =
														"{28A0092B-C50C-407E-A947-70E740481C1C}"
												})
										)
										{
											Embed = relationshipId,
											CompressionState =
												A.BlipCompressionValues.Print
										},
										new A.Stretch(
											new A.FillRectangle())),
									new PIC.ShapeProperties(
										new A.Transform2D(
											new A.Offset() { X = 0L, Y = 0L },
											new A.Extents() { Cx = 990000L, Cy = 792000L }),
										new A.PresetGeometry(
											new A.AdjustValueList()
										)
										{ Preset = A.ShapeTypeValues.Rectangle }))
							)
							{ Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
					)
					{
						DistanceFromTop = (UInt32Value)0U,
						DistanceFromBottom = (UInt32Value)0U,
						DistanceFromLeft = (UInt32Value)0U,
						DistanceFromRight = (UInt32Value)0U,
						EditId = "50D07946"
					});

			// Append the reference to body, the element should be in a Run.
			body.AppendChild(new Paragraph(new Run(element)));
		}

		[HttpPost("countView/{articleId}")]
		public async Task<IActionResult> CountArticleView(Guid articleId)
		{
			var article = _articleService.GetArticleByID(articleId);

			if (article == null)
			{
				return NotFound();
			}

			try
			{
				// Increment the view count of the article
				article.ViewCount++;

				// Update the article in the database with the incremented view count
				await _articleService.UpdateArticle(article);

				return Ok("Article view count incremented successfully.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred while updating the view count of the article: {ex.Message}");
			}
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