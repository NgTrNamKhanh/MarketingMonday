using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetArticles()
        {
            var articles = _mapper.Map<List<ArticleDTO>>(_articleService.GetArticles());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(articles);
        }
        [HttpGet("id/{articleId}")]
        public IActionResult GetArticleByID(Guid articleId)
        {
            if (!_articleService.ArticleExists(articleId))
                return NotFound();

            var article = _mapper.Map<ArticleDTO>(_articleService.GetArticleByID(articleId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(article);
        }

        [HttpGet("title/{articleTitle}")]
        public async Task<IActionResult> GetArticleByTitle(string articleTitle)
        {
            var articles = await _articleService.GetArticlesByTitle(articleTitle);

            if (articles == null || !articles.Any())
                return NotFound();

            var articleDTOs = _mapper.Map<IEnumerable<ArticleDTO>>(articles);

            return Ok(articleDTOs);
        }

        [HttpGet("faculty/{facultyId}")]
        public async Task<IActionResult> GetArticleByFacultyId(int facultyId)
        {
            var articles = await _articleService.GetArticlesByFacultyId(facultyId);

            if (articles == null || !articles.Any())
                return NotFound();

            var articleDTOs = _mapper.Map<IEnumerable<ArticleDTO>>(articles);

            return Ok(articleDTOs);
        }
        [HttpGet("studentConfimed/faculty/{facultyId}")]
        public async Task<IActionResult> GetStudentConfirmedArticle(int facultyId)
        {
            var articles = await _articleService.GetStudentConfirmedArticle(facultyId);

            if (articles == null || !articles.Any())
                return NotFound();

            var articleDTOs = _mapper.Map<IEnumerable<ArticleDTO>>(articles);

            return Ok(articleDTOs);
        }
        [HttpGet("studentOnHold/faculty/{facultyId}")]
        public async Task<IActionResult> GetStudentOnHoldArticle(int facultyId)
        {
            var articles = await _articleService.GetStudentOnHoldArticle(facultyId);

            if (articles == null || !articles.Any())
                return NotFound();

            var articleDTOs = _mapper.Map<IEnumerable<ArticleDTO>>(articles);

            return Ok(articleDTOs);
        }
        [HttpGet("studentOnHoldCommented/faculty/{facultyId}")]
        public async Task<IActionResult> GetStudentOnHoldCommentedArticle(int facultyId)
        {
            var articles = await _articleService.GetStudentOnHoldCommentedArticle(facultyId);

            if (articles == null || !articles.Any())
                return NotFound();

            var articleDTOs = _mapper.Map<IEnumerable<ArticleDTO>>(articles);

            return Ok(articleDTOs);
        }
        [HttpGet("studentOnHoldNotCommented/faculty/{facultyId}")]
        public async Task<IActionResult> GetStudentOnHoldNotCommentedArticle(int facultyId)
        {
            var articles = await _articleService.GetStudentOnHoldNotCommentedArticle(facultyId);

            if (articles == null || !articles.Any())
                return NotFound();

            var articleDTOs = _mapper.Map<IEnumerable<ArticleDTO>>(articles);

            return Ok(articleDTOs);
        }
        [HttpGet("studentOnHoldNotApproval/faculty/{facultyId}")]
        public async Task<IActionResult> GetStudentOnHoldNotApprovalArticle(int facultyId)
        {
            var articles = await _articleService.GetStudentNotApprovaldArticle(facultyId);

            if (articles == null || !articles.Any())
                return NotFound();

            var articleDTOs = _mapper.Map<IEnumerable<ArticleDTO>>(articles);

            return Ok(articleDTOs);
        }
        
        [HttpGet("student/{publishStatusId}/faculty/{facultyId}")]
        public async Task<IActionResult> GetArticleByPublishStatusIdAndFacultyId(int publishStatusId, int facultyId)
        {
            var articles = await _articleService.GetArticleByPublishStatusIdAndFacultyId(publishStatusId, facultyId);

            if (articles == null || !articles.Any())
                return NotFound();

            var articleDTOs = _mapper.Map<IEnumerable<ArticleDTO>>(articles);

            return Ok(articleDTOs);
        }

        //[HttpGet("DownloadImages")]
        //public IActionResult DownloadImages()
        //{
        //    var imagesDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

        //    if (!Directory.Exists(imagesDirectory))
        //    {
        //        // Handle the case where the "Images" folder doesn't exist
        //        return NotFound("The 'Images' folder does not exist.");
        //    }

        //    var imagePaths = Directory.GetFiles(imagesDirectory, "*.*", SearchOption.AllDirectories)
        //                               .Where(file => file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".jpeg") || file.ToLower().EndsWith(".png"))
        //                               .ToList();

        //    if (imagePaths.Count == 0)
        //    {
        //        // Handle the case where the "Images" folder is empty
        //        return NotFound("The 'Images' folder does not contain any images.");
        //    }

        //    var tempFolderPath = Path.Combine(Path.GetTempPath(), "Images");
        //    if (!Directory.Exists(tempFolderPath))
        //        Directory.CreateDirectory(tempFolderPath);

        //    foreach (var imagePath in imagePaths)
        //    {
        //        var relativePath = Path.GetRelativePath(imagesDirectory, imagePath);
        //        var destinationPath = Path.Combine(tempFolderPath, relativePath);

        //        var destinationDirectory = Path.GetDirectoryName(destinationPath);
        //        if (!Directory.Exists(destinationDirectory))
        //            Directory.CreateDirectory(destinationDirectory);

        //        System.IO.File.Copy(imagePath, destinationPath, true);
        //    }

        //    var zipPath = Path.Combine(Path.GetTempPath(), "Images.zip");
        //    ZipFile.CreateFromDirectory(tempFolderPath, zipPath, CompressionLevel.Fastest, true);

        //    var zipBytes = System.IO.File.ReadAllBytes(zipPath);
        //    return File(zipBytes, "application/zip", "Images.zip");
        //}

        [HttpGet("GetImages/{articleId}")]
        public async Task<IActionResult> GetImagesByArticleId(Guid articleId)
        {
            var article = _articleService.GetArticleByID(articleId);

            if (article == null)
            {
                return NotFound("Article not found");
            }

            if (string.IsNullOrEmpty(article.ImagePath))
            {
                return NotFound("No images found for the article");
            }

            var imagePaths = article.ImagePath.Split(';');

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

            if (imageBytesList.Count == 0)
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

            if (articleAdd.ImageFiles.Count > 0)
            {
                try
                {
                    if (!_articleService.IsValidImageFile(articleAdd.ImageFiles))
                    {
                        return BadRequest("Invalid image file format. Only PNG, JPG, JPEG, and GIF are allowed.");
                    }

                    var imagePaths = await _articleService.SaveImagesAsync(articleAdd.ImageFiles, articleMap.Id.ToString());
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
            //else
            //{
            //    return BadRequest("No image files were uploaded.");
            //}

            if (articleAdd.DocFiles.Length > 0)
            {
                try
                {
                    var docPath = await _articleService.SaveDocAsync(articleAdd.DocFiles, articleMap.Id.ToString());
                    articleMap.DocPath = docPath;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
            //else
            //{
            //    return BadRequest("Upload Doc Failed");
            //}

            _context.Articles.Add(articleMap);
            await _context.SaveChangesAsync();

            return Ok("Successfully added");
        }
        [HttpPut("{articleId}")]
        public async Task<ActionResult<Article>> UpdateArticle(Guid articleId, ListArticleDTO articleUpdate)
        {
            if (articleUpdate == null)
                return BadRequest(ModelState);

            if (!_articleService.ArticleExists(articleId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var articleMap = _mapper.Map<Article>(articleUpdate);
            articleMap.Id = articleId;

            if (articleUpdate.ImageFiles.Count > 0)
            {
                try
                {
                    if (!_articleService.IsValidImageFile(articleUpdate.ImageFiles))
                    {
                        return BadRequest("Invalid image file format. Only PNG, JPG, JPEG, and GIF are allowed.");
                    }

                    var imagePaths = await _articleService.SaveImagesAsync(articleUpdate.ImageFiles, articleMap.Id.ToString());
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
            //else
            //{
            //    return BadRequest("No image files were uploaded.");
            //}

            if (articleUpdate.DocFiles.Length > 0)
            {
                try
                {
                    var docPath = await _articleService.SaveDocAsync(articleUpdate.DocFiles, articleMap.Id.ToString());
                    articleMap.DocPath = docPath;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
            //else
            //{
            //    return BadRequest("Upload Doc Failed");
            //}

            _context.Articles.Update(articleMap);
            await _context.SaveChangesAsync();


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
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();

            return Ok("Successfully change article status");
        }

        [HttpDelete("{articleId}")]
        public IActionResult DeleteArticle(Guid articleId)
        {
            if (!_articleService.ArticleExists(articleId))
            {
                return NotFound();
            }

            var articleToDelete = _articleService.GetArticleByID(articleId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_articleService.DeleteArticle(articleToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting article");
            }

            return NoContent();
        }
    }
}
