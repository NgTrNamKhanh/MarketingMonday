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

            if (articleAdd.ImageFiles.Length > 0)
            {
                try
                {
                    if (!_articleService.IsValidImageFile(articleAdd.ImageFiles))
                    {
                        return BadRequest("Invalid image file format. Only PNG, JPG, JPEG, and GIF are allowed.");
                    }
                    var imagePath = await _articleService.SaveImageAsync(articleAdd.ImageFiles);
                    articleMap.ImagePath = imagePath;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
            else
            {
                return BadRequest("Upload Image Failed");
            }

            if (articleAdd.DocFiles.Length > 0)
            {
                try
                {
                    var docPath = await _articleService.SaveDocAsync(articleAdd.DocFiles);
                    articleMap.DocPath = docPath;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
            else
            {
                return BadRequest("Upload Doc Failed");
            }

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

            if (articleUpdate.ImageFiles.Length > 0)
            {
                try
                {
                    if (!_articleService.IsValidImageFile(articleUpdate.ImageFiles))
                    {
                        return BadRequest("Invalid image file format. Only PNG, JPG, JPEG, and GIF are allowed.");
                    }
                    var imagePath = await _articleService.SaveImageAsync(articleUpdate.ImageFiles);
                    articleMap.ImagePath = imagePath;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
            else
            {
                return BadRequest("Upload Image Failed");
            }

            if (articleUpdate.DocFiles.Length > 0)
            {
                try
                {
                    var docPath = await _articleService.SaveDocAsync(articleUpdate.DocFiles);
                    articleMap.DocPath = docPath;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
            else
            {
                return BadRequest("Upload Doc Failed");
            }
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
