using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO.Request;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Hubs;
using Comp1640_Final.Migrations;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IArticleService _aritcleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private static IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;

        public LikesController(ProjectDbContext context, 
            IMapper mapper,
            ILikeService likeService, 
            IArticleService aritcleService,
            IDislikeService dislikeService,
            ICommentService commentService,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext,
            INotificationService notificationService,
            IUserService userService,
            IWebHostEnvironment webHostEnvironment, IEmailService emailServivce)
        {
            _context = context;
            _mapper = mapper;
            _likeService = likeService;
            _dislikeService = dislikeService;
            _aritcleService = aritcleService;
            _commentService = commentService;
            _userManager = userManager;
            _hubContext = hubContext;
            _notificationService = notificationService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailServivce;
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
            //for noti
            

            if (existingLike != null)
            {
                var deleteLikeResult = await _likeService.DeleteLike(existingLike);

                if (!deleteLikeResult)
                {
                    return BadRequest("Error deleting existing like.");
                }
                return Ok("Existing like deleted successfully.");
            }

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
                //---------------- noti -----------------
                var article = _aritcleService.GetArticleByID(likeDto.ArticleId); //tìm article được tương tác
                var user = await _userManager.FindByIdAsync(likeDto.UserId); // tìm thằng tương tác với article đó
                var author = await _userManager.FindByIdAsync(article.StudentId); //tìm thg tác giả của article
                var sampleMessage = "liked your post";
                var oldNoti = await _notificationService.GetNotiByUserAndArticle(article.StudentId, likeDto.ArticleId, sampleMessage);
                // xóa noti cũ
                if (oldNoti != null)
                {
                    var deleteNoti = await _notificationService.DeleteNoti(oldNoti);
                    if (!deleteNoti)
                    {
                        return BadRequest("Error");
                    }
                    //return Ok("Successful");
                }
                // add noti mới
                var likeCount = await _likeService.GetArticleLikesCount(likeDto.ArticleId) -1;
                string message = user.FirstName + " " + user.LastName + " and " + likeCount.ToString() + " others liked your post: " + article.Title;
                var notification = new Notification
                {
                    UserId = article.StudentId,
                    UserInteractionId = likeDto.UserId,
                    ArticleId = likeDto.ArticleId,
                    Message = message,
                    IsAnonymous = false
                };
                var email = new EmailDTO
                {
                    Email = author.Email,
                    Subject = "Some one liked your post!",
                    Body = message,
                };
                _emailService.SendEmail(email);
                await _notificationService.PostNotification(notification);
                //await _hubContext.Clients.User(article.StudentId).SendAsync("ReceiveNotification", message);

                var notiResponse = _mapper.Map<NotificationResponse>(notification);
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712939986/tbzbwhyipuf7b4ep6dlm.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                UserNoti userNoti = new UserNoti
                {
                    Id = user.Id,
                    UserAvatar = cloudUserImage,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                if (notiResponse != null)
                {
                     notiResponse.UserNoti = userNoti;
                }
                //---------------- end noti -----------------
                return Ok(notiResponse);
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
                //------------------ noti -------------------
                var comment = _commentService.GetCommentById(likeDto.CommentId); // tìm comment
                var user = await _userManager.FindByIdAsync(likeDto.UserId); // tìm user tương tác
                var author = await _userManager.FindByIdAsync(comment.UserId); //tìm thg tác giả của article
                var sampleMessage = "liked your comment";
                var oldNoti = await _notificationService.GetNotiByUserAndComment(comment.UserId, likeDto.CommentId, sampleMessage);
                // xóa noti cũ
                if (oldNoti != null)
                {
                    var deleteNoti = await _notificationService.DeleteNoti(oldNoti);
                    if (!deleteNoti)
                    {
                        return BadRequest("Error");
                    }
                    //return Ok("Successful");
                }
                // add noti mứi
                var likeCount = await _likeService.GetCommentLikesCount(likeDto.CommentId) - 1;
                string message = user.FirstName + " " + user.LastName + " and " + likeCount.ToString() + " others liked your comment: " + comment.Content ;
                var notification = new Notification
                {
                    UserId = comment.UserId,
                    UserInteractionId = likeDto.UserId,
                    CommentId = likeDto.CommentId,
                    Message = message,
                    IsAnonymous = false
                };
                var email = new EmailDTO
                {
                    Email = author.Email,
                    Subject = "Some one liked your comment!",
                    Body = message,
                };
                _emailService.SendEmail(email);
                await _notificationService.PostNotification(notification);

                //await _hubContext.Clients.User(comment.UserId).SendAsync("ReceiveNotification", message);

                var notiResponse = _mapper.Map<NotificationResponse>(notification);
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call

                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712939986/tbzbwhyipuf7b4ep6dlm.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                UserNoti userNoti = new UserNoti
                {
                    Id = user.Id,
                    UserAvatar = cloudUserImage,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                if (notiResponse != null)
                {
                    notiResponse.UserNoti = userNoti;
                }

                //-------------------- end noti --------------------
                return Ok(notiResponse);
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
