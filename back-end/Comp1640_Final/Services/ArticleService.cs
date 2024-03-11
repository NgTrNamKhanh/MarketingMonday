using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Services
{
    public interface IAritcleService
    {
        ICollection<Article> GetArticles();
        Article GetArticleByID(Guid id);
        Task<IEnumerable<Article>> GetArticlesByTitle(string title);
        Task<IEnumerable<Article>> GetArticlesByFacultyId(int facultyID);
        bool ArticleExists(Guid articleId);
        bool DeleteArticle(Article article);
        bool Save();


    }
    public class AritcleService : IAritcleService
    {
        private readonly ProjectDbContext _context;

        public AritcleService(ProjectDbContext context)
        {
            _context = context;
        }
        public ICollection<Article> GetArticles()
        {
            return _context.Articles.OrderBy(p => p.ArticleId).ToList();
        }
        public Article GetArticleByID(Guid id)
        {
            return _context.Articles.Where(p => p.ArticleId == id).FirstOrDefault();
        }

        public async Task<IEnumerable<Article>> GetArticlesByTitle(string title)
        {
            return await _context.Articles.Where(p => p.Title.Contains(title)).ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetArticlesByFacultyId(int facultyID)
        {
            return await _context.Articles.Where(p => p.FacultyId == facultyID).ToListAsync();
        }
        public bool ArticleExists(Guid articleId)
        {
            return _context.Articles.Any(p => p.ArticleId == articleId);
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
    }
}
