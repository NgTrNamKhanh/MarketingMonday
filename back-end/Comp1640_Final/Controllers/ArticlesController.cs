using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Http;
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

        public ArticlesController(IAritcleService articleService,
            IMapper mapper,
            ProjectDbContext context)
        {
            _articleService = articleService;
            _mapper = mapper;
            _context = context;
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

        [HttpPost]
        public async Task<ActionResult<Article>> AddArticle(ArticleDTO articleAdd)
        {
            if (articleAdd == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var articleMap = _mapper.Map<Article>(articleAdd);
            articleMap.ArticleId = Guid.NewGuid();
            _context.Articles.Add(articleMap);
            await _context.SaveChangesAsync();

            return Ok("Successfully created");
        }
        [HttpPut("{articleId}")]
        public async Task<ActionResult<Article>> UpdateArticle(Guid articleId, ArticleDTO articleUpdate)
        {
            if (articleUpdate == null)
                return BadRequest(ModelState);

            if (!_articleService.ArticleExists(articleId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var articleMap = _mapper.Map<Article>(articleUpdate);
            articleMap.ArticleId = articleId;
            _context.Articles.Update(articleMap);
            await _context.SaveChangesAsync();


            return Ok("Successfully updated");
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
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }
    }
}
