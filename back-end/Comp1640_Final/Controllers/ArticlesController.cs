using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO.Request;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public ArticlesController(IAritcleService articleService,
            IMapper mapper,
            ProjectDbContext context,
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            ICommentService commentService)
        {
            _articleService = articleService;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _userService = userService;
            _commentService = commentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetArticles()
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
                articleDTO.StudentAvatar  = userImageBytes;
                articleDTO.StudentName = user.FirstName +" "+user.LastName;
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTOs.Add(articleDTO);
            }

            return Ok(articleDTOs);
        }
        [HttpGet("id/{articleId}")]
        public async Task<IActionResult> GetArticleByID(Guid articleId)
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
            articleDTO.StudentName = user.FirstName + " " + user.LastName;
            articleDTO.ImageBytes = imageBytes.ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(articleDTO);
        }

        [HttpGet("title/{articleTitle}")]
        public async Task<IActionResult> GetArticleByTitle(string articleTitle)
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
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTOs.Add(articleDTO);
            }
            
            return Ok(articleDTOs);
        }

        [HttpGet("faculty/{facultyId}")]
        public async Task<IActionResult> GetArticleByFacultyId(int facultyId)
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
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTOs.Add(articleDTO);
            }
            return Ok(articleDTOs);
        }
        [HttpGet("approved/faculty/{facultyId}")]
        public async Task<IActionResult> GetApprovedAticles(int facultyId)
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
                articleDTO.StudentAvatar = userImageBytes;
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTOs.Add(articleDTO);
            }
            return Ok(articleDTOs);
        }
        [HttpGet("status/{publishStatusId}/faculty/{facultyId}")]
        public async Task<IActionResult> GetArticleByPublishStatusIdAndFacultyId(int publishStatusId, int facultyId)
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
                articleDTO.StudentName = user.FirstName + " " + user.LastName;
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTOs.Add(articleDTO);
            }

            return Ok(articleDTOs);
        }

        [HttpGet("publishStatus/{publishStatusId}")]
        public async Task<IActionResult> GetArticleByPublishStatus(int publishStatusId)
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
        public async Task<ActionResult<Article>> AddArticle([FromForm]AddArticleDTO articleAdd)
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

            if (!await _articleService.UpdateArticle(article))
            {
                return BadRequest("Failed to update article.");
            }

            return Ok("Successfully add commnet for article");
        }

        [HttpDelete("{articleId}")]
        public IActionResult DeleteArticle(Guid articleId)
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

            if (!_articleService.DeleteArticle(articleToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting article");
            }

            return Ok("Successfully delete article");
        }
    }
}
