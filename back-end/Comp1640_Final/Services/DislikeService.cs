using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Services
{
    public interface IDislikeService
    {
        Task<ICollection<Dislike>> GetArticleDislikes(Guid articleId);
        Task<ICollection<Dislike>> GetCommentDislikes(Guid commentId);
        Task<Dislike> GetDislikeByArticleAndUser(Guid articleId, string userId);
        Task<Dislike> GetDislikeByCommentAndUser(Guid commentId, string userId);
        Task<bool> PostDislike(Dislike dislike);
        Task<int> GetDislikesCount(Guid articleId);
        Task<bool> Save();
        Task<bool> DeleteDislike(Dislike dislike);
    }
    public class DislikeService : IDislikeService
    {
        private readonly ProjectDbContext _context;

        public DislikeService(ProjectDbContext context)
        {
            _context = context;
        }
        public async Task<bool> DeleteDislike(Dislike dislike)
        {
            _context.Remove(dislike);
            return await Save();
        }

        public async Task<ICollection<Dislike>> GetArticleDislikes(Guid articleId)
        {
            return await _context.Dislikes.Where(p => p.ArticleId == articleId).ToListAsync();
        }
        public async Task<ICollection<Dislike>> GetCommentDislikes(Guid commentId)
        {
            return await _context.Dislikes.Where(p => p.CommentId == commentId).ToListAsync();
        }
        public async Task<Dislike> GetDislikeByArticleAndUser(Guid articleId, string userId)
        {
            return await _context.Dislikes.FirstOrDefaultAsync(p => p.ArticleId == articleId && p.UserId == userId);
        }
        public async Task<Dislike> GetDislikeByCommentAndUser(Guid commentId, string userId)
        {
            return await _context.Dislikes.FirstOrDefaultAsync(p => p.CommentId == commentId && p.UserId == userId);
        }
        public async Task<int> GetDislikesCount(Guid articleId)
        {
            return await _context.Dislikes.CountAsync(p => p.ArticleId == articleId);
        }

        public async Task<bool> PostDislike(Dislike dislike)
        {
            _context.Dislikes.Add(dislike);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
