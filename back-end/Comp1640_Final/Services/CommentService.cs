using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Services
{

    public interface ICommentService
    {
        Task<ICollection<Comment>> GetComments();

        Comment GetCommentById(Guid id);
        Task<ICollection<Comment>> GetParentComments(Guid articleId);
        Task<ICollection<Comment>> GetReplies(Guid parentId);
        Task<bool> PostComment(Comment cmt);

        Task<bool> EditComment(Comment cmt);
        Task<int> GetCommentsCount(Guid  articleId);
        Task<bool> Save();

        Task<bool>  DeleteComment(Comment cmt);    
        bool CommentExists(Guid cmtId);    
    }

    public class CommentService : ICommentService
    {
        private readonly ProjectDbContext _context;

        public CommentService(ProjectDbContext context)
        {
            _context = context;
        }

        public bool CommentExists(Guid cmtId)
        {
            return _context.Comments.Any(p => p.Id == cmtId);
        }

        public async Task<bool> DeleteComment(Comment cmt)
        {
            _context.Remove(cmt);
            return await Save();
        }

        public async Task<bool> EditComment(Comment cmt)
        {
            _context.Comments.Update(cmt);
            return await Save();
        }

        public Comment GetCommentById(Guid id)
        {
            return _context.Comments.Where(p => p.Id == id).FirstOrDefault();
        }

        public async Task<ICollection<Comment>> GetComments()
        {
            return await _context.Comments.OrderByDescending(p => p.CreateOn).ToListAsync();
        }

        public async Task<int> GetCommentsCount(Guid articleId)
        {
            return await _context.Comments.CountAsync(p => p.ArticleId == articleId);
        }

        public async Task<ICollection<Comment>> GetParentComments(Guid articleId)
        {
            return await _context.Comments.Where(p => p.ArticleId == articleId && p.ParentCommentId == null).OrderByDescending(p => p.CreateOn).ToListAsync();
        }

        public async Task<ICollection<Comment>> GetReplies(Guid parentId)
        {
            return await _context.Comments.Where(p => p.ParentCommentId == parentId).OrderByDescending(p => p.CreateOn).ToListAsync();
        }

        public async Task<bool> PostComment(Comment cmt)
        {
            _context.Comments.Add(cmt);
            return await Save();
        }


        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
