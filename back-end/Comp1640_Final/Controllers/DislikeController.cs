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
        private readonly IArticleService _aritcleService;
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
            IArticleService aritcleService, 
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
            //for noti
            

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
                var sampleMessage = "disliked your post";
                var oldNoti = await _notificationService.GetNotiByUserAndArticle(article.StudentId, dislikeDto.ArticleId, sampleMessage);
                if (oldNoti != null)
                {
                    var deleteNoti = await _notificationService.DeleteNoti(oldNoti);
                    if (!deleteNoti)
                    {
                        return BadRequest("Error");
                    }
                    //return Ok("Successful");
                }

                var dislikeCount = await _dislikeService.GetArticleDislikesCount(dislikeDto.ArticleId);
                string message = user.FirstName + " " + user.LastName + " and " + dislikeCount.ToString() + " others disliked your post: " + article.Title;
                var notification = new Notification
                {
                    UserId = article.StudentId,
                    UserInteractionId = dislikeDto.UserId,
                    ArticleId = dislikeDto.ArticleId,
                    Message = message,
                    IsAnonymous = false
                };
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
        public async Task<ActionResult<Dislike>> PostCommentDislike(CommentInteractDTO dislikeDto)
        {
            var existingDislike = await _dislikeService.GetDislikeByCommentAndUser(dislikeDto.CommentId, dislikeDto.UserId);
            //noti info


            if (existingDislike != null)
            {
                var deleteDislikeResult = await _dislikeService.DeleteDislike(existingDislike);
                
                if (!deleteDislikeResult)
                {
                    return BadRequest("Error deleting existing dislike.");
                }
                return Ok("Existing dislike deleted successfully.");
            }
            
            var dislike = _mapper.Map<Dislike>(dislikeDto);
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
                var comment = _commentService.GetCommentById(dislikeDto.CommentId); // tìm comment
                var user = await _userManager.FindByIdAsync(dislikeDto.UserId); // tìm user tương tác
                var sampleMessage = "disliked your comment";
                var oldNoti = await _notificationService.GetNotiByUserAndComment(comment.UserId, dislikeDto.CommentId, sampleMessage);
                if (oldNoti != null)
                {
                    var deleteNoti = await _notificationService.DeleteNoti(oldNoti);
                    if (!deleteNoti)
                    {
                        return BadRequest("Error");
                    }
                    //return Ok("Successful");
                }
                var dislikeCount = await _dislikeService.GetCommentDislikesCount(dislikeDto.CommentId) -1;
                string message = user.FirstName + " " + user.LastName + " and " + dislikeCount.ToString() + " others disliked your comment: " + comment.Content;
                var notification = new Notification
                {
                    UserId = comment.UserId,
                    UserInteractionId = dislikeDto.UserId,
                    CommentId = dislikeDto.CommentId,
                    Message = message,
                    IsAnonymous = false
                };
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
        public async Task<IActionResult> DeleteDislike(Guid id)
        {
            var dislike = await _context.Dislikes.FirstOrDefaultAsync(l => l.Id == id);
            if (dislike == null)
            {
                return NotFound();
            }
            var result = await _dislikeService.DeleteDislike(dislike);
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
