using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Services
{

    public interface ILikeService
    {
        Task<ICollection<Like>> GetLikes(Guid articleId);
        Task<bool> PostLike(Like like);
        Task<int> GetLikesCount(Guid articleId);
        Task<bool> Save();
        Task<bool> DeleteLike(Like like);
    }

    public class LikeService : ILikeService
    {
        private readonly ProjectDbContext _context;

        public LikeService(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteLike(Like like)
        {
            _context.Remove(like);
            return await Save();
        }

        public async Task<ICollection<Like>> GetLikes(Guid articleId)
        {
            return await _context.Likes.Where(p => p.ArticleId == articleId).ToListAsync();
        }

        public async Task<int> GetLikesCount(Guid articleId)
        {
            return await _context.Likes.CountAsync(p => p.ArticleId == articleId);
        }

        public async Task<bool> PostLike(Like like)
        {
            _context.Likes.Add(like);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
