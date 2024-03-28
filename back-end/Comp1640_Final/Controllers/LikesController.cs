using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO.Request;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Hubs;
using Comp1640_Final.Migrations;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Controllers
{
    [Route("api/like")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILikeService _likeService;
        private readonly IDislikeService _dislikeService;
        private readonly IAritcleService _aritcleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;

        public LikesController(ProjectDbContext context, 
            IMapper mapper,
            ILikeService likeService, 
            IAritcleService aritcleService,
            IDislikeService dislikeService,
            ICommentService commentService,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _mapper = mapper;
            _likeService = likeService;
            _dislikeService = dislikeService;
            _aritcleService = aritcleService;
            _commentService = commentService;
            _userManager = userManager;
            _hubContext = hubContext;
        }
        //[HttpGet("count/article/{articleId}")]
        //public async Task<IActionResult> GetArticleLikesCount(Guid articleId)
        //{
        //    try
        //    {
        //        int likesCount = await _likeService.GetArticleLikesCount(articleId);
        //        return Ok(likesCount);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Failed to get likes count: {ex.Message}");
        //    }
        //}
        [HttpGet("count/comment/{articleId}")]
        public async Task<IActionResult> GetCommentLikesCount(Guid commentId)
        {
            try
            {
                int likesCount = await _likeService.GetCommentLikesCount(commentId);
                return Ok(likesCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to get likes count: {ex.Message}");
            }
        }
        [HttpGet("article/{articleId}")]
        public async Task<IActionResult> GetArticleLikes(Guid articleId)
        {
            if (!_aritcleService.ArticleExists(articleId))
            {
                return NotFound();
            }
            var likes = await _likeService.GetArticleLikes(articleId);
            var likeResponse = _mapper.Map<List<InteractResponse>>(likes);
            return Ok(likeResponse);
        }

        [HttpGet("comment/{commentId}")]
        public async Task<IActionResult> GetCommentLikes(Guid commentId)
        {
            if (!_commentService.CommentExists(commentId))
            {
                return NotFound();
            }
            var likes = await _likeService.GetCommentLikes(commentId);
            var likeResponse = _mapper.Map<List<InteractResponse>>(likes);
            return Ok(likeResponse);
        }

        [HttpPost("article")]
        public async Task<ActionResult<Like>> PostArticleLike( ArticleInteractDTO likeDto)
        {
            var existingLike = await _likeService.GetLikeByArticleAndUser(likeDto.ArticleId, likeDto.UserId);

            if (existingLike != null)
            {
                var deleteLikeResult = await _likeService.DeleteLike(existingLike);
                if (!deleteLikeResult)
                {
                    return BadRequest("Error deleting existing like.");
                }
                return Ok("Existing like deleted successfully.");
            }
            var article = _aritcleService.GetArticleByID(likeDto.ArticleId); //tìm article được tương tác
            var user = await _userManager.FindByIdAsync(likeDto.UserId); // tìm thằng tương tác với article đó
            await _hubContext.Clients.User(article.StudentId).SendAsync("ReceiveNotification",
    user.FirstName + " " + user.LastName + " liked your post");
            //await NotifyUser(article.StudentId, user.FirstName + " " + user.LastName + " liked your post");

            var like = _mapper.Map<Like>(likeDto);
            like.Id = Guid.NewGuid();
            like.Date = DateTime.Now;

            var postResult = await _likeService.PostLike(like);
            if (!postResult)
            {
                return BadRequest("Error posting new like.");
            }
            else
            {
                return Ok("Successful");
            }
        }
        [HttpPost("comment")]
        public async Task<ActionResult<Like>> PostCommentLike(CommentInteractDTO likeDto)
        {
            var existingLike = await _likeService.GetLikeByCommentAndUser(likeDto.CommentId, likeDto.UserId);

            if (existingLike != null)
            {
                var deleteLikeResult = await _likeService.DeleteLike(existingLike);
                if (!deleteLikeResult)
                {
                    return BadRequest("Error deleting existing like.");
                }
                return Ok("Existing like deleted successfully.");
            }
            var comment = _commentService.GetCommentById(likeDto.CommentId); // tìm comment
            //var userComment = await _userManager.FindByIdAsync(comment.UserId); //tìm user của comment đó
            var user =  await _userManager.FindByIdAsync(likeDto.UserId); // tìm user tương tác
            await _hubContext.Clients.User(comment.UserId).SendAsync("ReceiveNotification", 
                user.FirstName + " " + user.LastName + " liked your post");
            //await NotifyUser(comment.UserId, user.FirstName + " " + user.LastName + " liked your post");

            var like = _mapper.Map<Like>(likeDto);
            like.Id = Guid.NewGuid();
            like.Date = DateTime.Now;
            var result = await _likeService.PostLike(like);
            if (!result)
            {
                return BadRequest("Error");
            }
            else
            {
                return Ok("Successful");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(Guid id)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(l => l.Id == id);
            if (like == null)
            {
                return NotFound(); 
            }
            var result = await _likeService.DeleteLike(like);
            if (!result)
            {
                return BadRequest("Error");
            }
            else
            {
                return Ok("Successful");
            }
        }

        //public async Task NotifyUser(string userId, string message)
        //{
        //    await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        //}

    }
}
