using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
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

        public ArticlesController(IAritcleService articleService,
            IMapper mapper,
            ProjectDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _articleService = articleService;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> GetArticles()
        {
            var articles = _articleService.GetArticles();
            var articleDTOs = new List<ArticleDTO>();

            foreach (var article in articles)
            {
                var articleDTO = _mapper.Map<ArticleDTO>(article);
                var imageBytes = await _articleService.GetImagesByArticleId(article.Id);
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTOs.Add(articleDTO);
            }

            return Ok(articleDTOs);
        }
        [HttpGet("id/{articleId}")]
        public async Task<IActionResult> GetArticleByID(Guid articleId)
        {
            if (!_articleService.ArticleExists(articleId))
                return NotFound();

            var article = _mapper.Map<ArticleDTO>(_articleService.GetArticleByID(articleId));
            var imageBytes = await _articleService.GetImagesByArticleId(articleId);
            article.ImageBytes = imageBytes.ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(article);
        }

        [HttpGet("title/{articleTitle}")]
        public async Task<IActionResult> GetArticleByTitle(string articleTitle)
        {
            var articles = await _articleService.GetArticlesByTitle(articleTitle); 

            if (articles == null || !articles.Any())
                return NotFound(); var articleDTOs = new List<ArticleDTO>();

            foreach (var article in articles)
            {
                var articleDTO = _mapper.Map<ArticleDTO>(article);
                var imageBytes = await _articleService.GetImagesByArticleId(article.Id);
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
                return NotFound();

            var articleDTOs = new List<ArticleDTO>();

            foreach (var article in articles)
            {
                var articleDTO = _mapper.Map<ArticleDTO>(article);
                var imageBytes = await _articleService.GetImagesByArticleId(article.Id);
                articleDTO.ImageBytes = imageBytes.ToList();
                articleDTOs.Add(articleDTO);
            }
            return Ok(articleDTOs);
        }
        
        [HttpGet("student/{publishStatusId}/faculty/{facultyId}")]
        public async Task<IActionResult> GetArticleByPublishStatusIdAndFacultyId(int publishStatusId, int facultyId)
        {
            var articles = await _articleService.GetArticleByPublishStatusIdAndFacultyId(publishStatusId, facultyId);

            if (articles == null || !articles.Any())
                return NotFound();

            var articleDTOs = new List<ArticleDTO>();

            foreach (var article in articles)
            {
                var articleDTO = _mapper.Map<ArticleDTO>(article);
                var imageBytes = await _articleService.GetImagesByArticleId(article.Id);
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

            var articleDTOs = new List<ArticleDTO>();

            foreach (var article in articles)
            {
                var articleDTO = _mapper.Map<ArticleDTO>(article);
                var imageBytes = await _articleService.GetImagesByArticleId(article.Id);
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

        [HttpPost]
        public async Task<ActionResult<Article>> AddArticle([FromForm]ListArticleDTO articleAdd)
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
        public async Task<ActionResult<Article>> UpdateArticle(Guid articleId, [FromForm] ListArticleDTO articleUpdate)
        {
            if (articleUpdate == null)
                return BadRequest(ModelState);

            if (!_articleService.ArticleExists(articleId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var articleMap = _mapper.Map<Article>(articleUpdate);
            articleMap.Id = articleId;
            articleMap.UploadDate = DateTime.Now;
            var article = _articleService.GetArticleByID(articleId);

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
