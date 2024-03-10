using Comp1640_Final.Data;
using Comp1640_Final.Models;

namespace Comp1640_Final.Services
{
    public interface IAritcleService
    {
        ICollection<Article> GetArticles();
        Article GetArticleByID(Guid id);
        Article GetArticleByTitle(string title);
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

        public Article GetArticleByTitle(string title)
        {
            return _context.Articles.Where(p => p.Title == title).FirstOrDefault();
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
