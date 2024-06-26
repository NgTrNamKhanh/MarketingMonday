﻿using Comp1640_Final.Data;
using Comp1640_Final.Hubs;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Services
{

    public interface ILikeService
    {
        Task<ICollection<Like>> GetArticleLikes(Guid articleId);
        Task<ICollection<Like>> GetCommentLikes(Guid commentId);
        Task<Like> GetLikeByArticleAndUser(Guid articleId, string userId);
        Task<Like> GetLikeByCommentAndUser(Guid commentId, string userId);
        Task<bool> IsArticleLiked(string userId, Guid articleId);
        Task<bool> IsCommentLiked(string userId, Guid commentId);

        Task<bool> PostLike(Like like);
        Task<int> GetArticleLikesCount(Guid articleId);
        Task<int> GetCommentLikesCount(Guid commentId);
        Task<bool> Save();
        Task<bool> DeleteLike(Like like);

        //Task NotifyUser(string userId, string message);
    }

    public class LikeService : ILikeService
    {
        private readonly ProjectDbContext _context;
        //private readonly IHubContext<NotificationHub> _hubContext;

        public LikeService(ProjectDbContext context)
        {
            _context = context;
            //_hubContext = hubContext;
        }

        public async Task<bool> DeleteLike(Like like)
        {
            _context.Remove(like);
            return await Save();
        }

        public async Task<ICollection<Like>> GetArticleLikes(Guid articleId)
        {
            return await _context.Likes.Where(p => p.ArticleId == articleId).ToListAsync();
        }
        public async Task<ICollection<Like>> GetCommentLikes(Guid commentId)
        {
            return await _context.Likes.Where(p => p.CommentId == commentId).ToListAsync();
        }
        public async Task<Like> GetLikeByArticleAndUser(Guid articleId, string userId)
        {
            return await _context.Likes.FirstOrDefaultAsync(p => p.ArticleId == articleId && p.UserId == userId);
        }
        public async Task<Like> GetLikeByCommentAndUser(Guid commentId, string userId)
        {
            return await _context.Likes.FirstOrDefaultAsync(p => p.CommentId == commentId && p.UserId == userId);
        }
        public async Task<int> GetArticleLikesCount(Guid articleId)
        {
            return await _context.Likes.CountAsync(p => p.ArticleId == articleId);
        }
        public async Task<bool> IsArticleLiked(string userId, Guid articleId)
        {
            return await _context.Likes.AnyAsync(p => p.ArticleId == articleId && p.UserId == userId);
        }
        public async Task<int> GetCommentLikesCount(Guid commentId)
        {
            return await _context.Likes.CountAsync(p => p.CommentId == commentId);
        }

        public async Task<bool> IsCommentLiked(string userId, Guid commentId)
        {
            return await _context.Likes.AnyAsync(p => p.CommentId == commentId && p.UserId == userId);
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

        //public async Task NotifyUser(string userId, string message)
        //{
        //    await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        //}
    }
}
