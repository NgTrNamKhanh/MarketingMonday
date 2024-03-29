using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO.Request;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Hubs;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Permissions;

namespace Comp1640_Final.Controllers
{
    [Route("api/dislike")]
    [ApiController]
    public class DislikeController : ControllerBase
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDislikeService _dislikeService;
        private readonly IAritcleService _aritcleService;
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private static IWebHostEnvironment _webHostEnvironment;

        public DislikeController(ProjectDbContext context, 
            IMapper mapper, 
            IDislikeService dislikeService,
            IAritcleService aritcleService, 
            ILikeService likeService,
            ICommentService commentService,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext,
            INotificationService notificationService,
            IUserService userService,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _dislikeService = dislikeService;
            _aritcleService = aritcleService;
            _likeService = likeService;
            _commentService = commentService;
            _userManager = userManager;
            _hubContext = hubContext;
            _notificationService = notificationService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }
        //[HttpGet("count/article/{articleId}")]
        //public async Task<IActionResult> GetArticleDislikesCount(Guid articleId)
        //{
        //    try
        //    {
        //        int likesCount = await _dislikeService.GetArticleDislikesCount(articleId);
        //        return Ok(likesCount);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Failed to get dislikes count: {ex.Message}");
        //    }
        //}
        [HttpGet("count/comment/{articleId}")]
        public async Task<IActionResult> GetCommentDislikesCount(Guid commentId)
        {
            try
            {
                int likesCount = await _dislikeService.GetCommentDislikesCount(commentId);
                return Ok(likesCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to get dislikes count: {ex.Message}");
            }
        }
        [HttpGet("article/{articleId}")]
        public async Task<IActionResult> GetArticleDislikes(Guid articleId)
        {
            if (!_aritcleService.ArticleExists(articleId))
            {
                return NotFound();
            }
            var dislikes = await _dislikeService.GetArticleDislikes(articleId);
            var likeResponse = _mapper.Map<List<InteractResponse>>(dislikes);
            return Ok(likeResponse);
        }
        [HttpGet("comment/{commentId}")]
        public async Task<IActionResult> GetCommentDislikes(Guid commentId)
        {
            if (!_commentService.CommentExists(commentId))
            {
                return NotFound();
            }
            var dislikes = await _dislikeService.GetCommentDislikes(commentId);
            var likeResponse = _mapper.Map<List<InteractResponse>>(dislikes);
            return Ok(likeResponse);
        }

        [HttpPost("article")]
        public async Task<ActionResult<Dislike>> PostArticleDislike(ArticleInteractDTO dislikeDto)
        {
            var existingDislike = await _dislikeService.GetDislikeByArticleAndUser(dislikeDto.ArticleId, dislikeDto.UserId);

            if (existingDislike != null)
            {
                var deleteDislikeResult = await _dislikeService.DeleteDislike(existingDislike);
                if (!deleteDislikeResult)
                {
                    return BadRequest("Error deleting existing dislike.");
                }
                return Ok("Dislike deleted successfully.");
            }
            var dislike = _mapper.Map<Dislike>(dislikeDto);
            dislike.Id = Guid.NewGuid();
            dislike.Date = DateTime.Now;

            var postResult = await _dislikeService.PostDislike(dislike);
            if (!postResult)
            {
                return BadRequest("Error posting new dislike.");
            }
            else
            {
                //---------------- noti -----------------

                var article = _aritcleService.GetArticleByID(dislikeDto.ArticleId); //tìm article được tương tác
                var user = await _userManager.FindByIdAsync(dislikeDto.UserId); // tìm thằng tương tác với article đó
                string message = user.FirstName + " " + user.LastName + " disliked your post";
                var notification = new Notification
                {
                    UserId = article.StudentId,
                    UserInteractionId = dislikeDto.UserId,
                    Message = message,
                };
                await _notificationService.PostNotification(notification);
                await _hubContext.Clients.User(article.StudentId).SendAsync("ReceiveNotification", message);

                var notiResponse = _mapper.Map<NotificationResponse>(notification);
                var userImageBytes = await _userService.GetImagesByUserId(user.Id);
                if (userImageBytes == null)
                {
                    var defaultImageFileName = "default-avatar.jpg";
                    var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                    userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                }
                UserNoti userNoti = new UserNoti
                {
                    Id = user.Id,
                    UserAvatar = userImageBytes,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                notiResponse.UserNoti = userNoti;
                //---------------- end noti -----------------
                return Ok(notiResponse);
            }

        }
        [HttpPost("comment")]
        public async Task<ActionResult<Dislike>> PostCommentDislike(CommentInteractDTO likeDto)
        {
            var existingDislike = await _dislikeService.GetDislikeByCommentAndUser(likeDto.CommentId, likeDto.UserId);

            if (existingDislike != null)
            {
                var deleteDislikeResult = await _dislikeService.DeleteDislike(existingDislike);
                if (!deleteDislikeResult)
                {
                    return BadRequest("Error deleting existing dislike.");
                }
                return Ok("Existing dislike deleted successfully.");
            }

            var dislike = _mapper.Map<Dislike>(likeDto);
            dislike.Id = Guid.NewGuid();
            dislike.Date = DateTime.Now;
            var result = await _dislikeService.PostDislike(dislike);
            if (!result)
            {
                return BadRequest("Error");
            }
            else
            {
                //------------------ noti -------------------

                var comment = _commentService.GetCommentById(likeDto.CommentId); // tìm comment
                var user = await _userManager.FindByIdAsync(likeDto.UserId); // tìm user tương tác

                string message = user.FirstName + " " + user.LastName + " disliked your comment";
                var notification = new Notification
                {
                    UserId = comment.UserId,
                    UserInteractionId = likeDto.UserId,
                    Message = message,
                };
                await _notificationService.PostNotification(notification);

                await _hubContext.Clients.User(comment.UserId).SendAsync("ReceiveNotification", message);

                var notiResponse = _mapper.Map<NotificationResponse>(notification);
                var userImageBytes = await _userService.GetImagesByUserId(user.Id);
                if (userImageBytes == null)
                {
                    var defaultImageFileName = "default-avatar.jpg";
                    var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                    userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                }
                UserNoti userNoti = new UserNoti
                {
                    Id = user.Id,
                    UserAvatar = userImageBytes,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                notiResponse.UserNoti = userNoti;

                //-------------------- end noti --------------------
                return Ok(notiResponse);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDislike(Guid id)
        {
            var like = await _context.Dislikes.FirstOrDefaultAsync(l => l.Id == id);
            if (like == null)
            {
                return NotFound();
            }
            var result = await _dislikeService.DeleteDislike(like);
            if (!result)
            {
                return BadRequest("Error");
            }
            else
            {
                return Ok("Successful");
            }
        }
    }
}
