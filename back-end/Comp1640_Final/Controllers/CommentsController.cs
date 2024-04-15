using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO.Request;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Migrations;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Comp1640_Final.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommentService _commentService;
        private static IWebHostEnvironment _webHostEnvironment;
        private readonly IUserService _userService;
        private readonly ILikeService _likeService;
        private readonly IDislikeService _dislikeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IArticleService _articleService;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public CommentsController(ProjectDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, 
            ICommentService commentService, IWebHostEnvironment webHostEnvironment, IUserService userService, ILikeService likeService, 
            IDislikeService dislikeService, IHttpContextAccessor httpContextAccessor,
            IArticleService articleService, INotificationService notificationService, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _commentService = commentService;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
            _likeService = likeService;
            _dislikeService  = dislikeService;
            _httpContextAccessor = httpContextAccessor;
            _articleService = articleService;
            _notificationService = notificationService;
            _emailService = emailService;

        }

        [HttpGet("getComment")]
        public async Task<IActionResult> GetComments(string userId)
        {
            var comments = await _commentService.GetComments();
            var currentUser = await _userManager.FindByIdAsync(userId);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);
            var isGuestOrStudent = currentUserRole.Contains("Guest") || currentUserRole.Contains("Student");

            if (comments == null)
            {
                return BadRequest("No comments found");
            }
            else
            {
                List<CommentResponse> commentResponses = new List<CommentResponse>();
                foreach (var comment in comments)
                {
                    var commentResponse = _mapper.Map<CommentResponse>(comment);
                    ICollection<Comment> replies = await _commentService.GetReplies(comment.Id);
                    bool hasReplies = replies.Count > 0;
                    if (commentResponse != null)
                    {
                        commentResponse.hasReplies = hasReplies;
                    }
                    var user = await _userManager.FindByIdAsync(comment.UserId);
                    if (user != null)
                    {
                        var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call
                        var firstName = user.FirstName;
                        var lastName = user.LastName;
                        var userIden = user.Id;
                        // If imageBytes is null, read the default image file
                        if (cloudUserImage == null)
                        {
                            var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712939986/tbzbwhyipuf7b4ep6dlm.jpg";
                            cloudUserImage = defaultImageFileName;
                        }
                        if (comment.IsAnonymous == true && userId != comment.UserId && isGuestOrStudent)
                        {
                            var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712937035/ke2iqrl0rqnxozhxp378.png";
                            cloudUserImage = defaultImageFileName;
                            firstName = "Anonymous";
                            lastName = "";
                            userIden = "";
                        }
                        UserComment userComment = new UserComment
						{
							Id = userIden,
							UserAvatar = cloudUserImage,
							FirstName = firstName,
							LastName = lastName,
						};
                        if (commentResponse != null)
                        {
                            commentResponse.UserComment = userComment;
                        }
                    }
                    commentResponse.LikesCount = await _likeService.GetCommentLikesCount(comment.Id);
                    commentResponse.DislikesCount = await _dislikeService.GetCommentDislikesCount(comment.Id);
                    commentResponse.IsLiked = await _likeService.IsCommentLiked(userId, comment.Id);
                    commentResponse.IsDisliked = await _dislikeService.IsCommentDisLiked(userId, comment.Id);
                    commentResponses.Add(commentResponse);
                }

                return Ok(commentResponses);
            }
        }

        [HttpGet("getParentComments")]
        public async Task<IActionResult> GetParentComments(Guid articleId, string userId)
        {
            var comments =  await _commentService.GetParentComments(articleId);
            var currentUser = await _userManager.FindByIdAsync(userId);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);
            var isGuestOrStudent = currentUserRole.Contains("Guest") || currentUserRole.Contains("Student");

            if (comments == null)
            {
                return BadRequest("No comments found");
            }
            else 
            {
                List<CommentResponse> commentResponses = new List<CommentResponse>();
                foreach (var comment in comments) 
                {

                    var commentResponse = _mapper.Map<CommentResponse>(comment);
                    ICollection<Comment> replies = await _commentService.GetReplies(comment.Id);
                    bool hasReplies = replies.Count > 0;
                    commentResponse.hasReplies = hasReplies;
                    var user = await _userManager.FindByIdAsync(comment.UserId);
                    if (user != null)
                    {
                        var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call
                        var firstName = user.FirstName;
                        var lastName = user.LastName;
                        var userIden = user.Id;
                        // If imageBytes is null, read the default image file
                        if (cloudUserImage == null)
                        {
                            var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712939986/tbzbwhyipuf7b4ep6dlm.jpg";
                            cloudUserImage = defaultImageFileName;
                        }
                        if (comment.IsAnonymous == true && userId != comment.UserId && isGuestOrStudent)
                        {
                            var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712937035/ke2iqrl0rqnxozhxp378.png";
                            cloudUserImage = defaultImageFileName;
                            firstName = "Anonymous";
                            lastName = "";
                            userIden = "";
                        }
                        UserComment userComment = new UserComment
                        {
                            Id = userIden,
                            UserAvatar = cloudUserImage,
                            FirstName = firstName,
                            LastName = lastName,
                        };
                        commentResponse.UserComment = userComment;
                    }
                    commentResponse.LikesCount = await _likeService.GetCommentLikesCount(comment.Id);
                    commentResponse.DislikesCount = await _dislikeService.GetCommentDislikesCount(comment.Id);
                    commentResponse.IsLiked = await _likeService.IsCommentLiked(userId, comment.Id);
                    commentResponse.IsDisliked = await _dislikeService.IsCommentDisLiked(userId, comment.Id);

                    commentResponses.Add(commentResponse);
                }

                return Ok(commentResponses);
            }
        }

        [HttpGet("getReplies")]
        public async Task<IActionResult> GetReplies(Guid parentId, string userId)
        {
            var comments = await _commentService.GetReplies(parentId);
            var currentUser = await _userManager.FindByIdAsync(userId);
            var currentUserRole = await _userManager.GetRolesAsync(currentUser);
            var isGuestOrStudent = currentUserRole.Contains("Guest") || currentUserRole.Contains("Student");

            if (comments == null)
            {
                return BadRequest("No comments found");
            }
            else
            {
                List<CommentResponse> commentResponses = new List<CommentResponse>();
                foreach (var comment in comments)
                {
                    var commentResponse = _mapper.Map<CommentResponse>(comment);
                    ICollection<Comment> replies = await _commentService.GetReplies(comment.Id);
                    bool hasReplies = replies.Count > 0;
                    commentResponse.hasReplies = hasReplies;
                    var user = await _userManager.FindByIdAsync(comment.UserId);
                    if (user != null)
                    {
                        var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call
                        var firstName = user.FirstName;
                        var lastName = user.LastName;
                        var userIden = user.Id;
                        // If imageBytes is null, read the default image file
                        if (cloudUserImage == null)
                        {
                            var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712939986/tbzbwhyipuf7b4ep6dlm.jpg";
                            cloudUserImage = defaultImageFileName;
                        }
                        if (comment.IsAnonymous == true && userId != comment.UserId && isGuestOrStudent)
                        {
                            var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712937035/ke2iqrl0rqnxozhxp378.png";
                            cloudUserImage = defaultImageFileName;
                            firstName = "Anonymous";
                            lastName = "";
                            userIden = "";
                        }
                        UserComment userComment = new UserComment
                        {
                            Id = userIden,
                            UserAvatar = cloudUserImage,
                            FirstName = firstName,
                            LastName = lastName,
                        };
                        commentResponse.UserComment = userComment;
                    }
                    commentResponse.LikesCount = await _likeService.GetCommentLikesCount(comment.Id);
                    commentResponse.DislikesCount = await _dislikeService.GetCommentDislikesCount(comment.Id);
                    commentResponse.IsLiked = await _likeService.IsCommentLiked(userId, comment.Id);
                    commentResponse.IsDisliked = await _dislikeService.IsCommentDisLiked(userId, comment.Id);
                    commentResponses.Add(commentResponse);
                }

                return Ok(commentResponses);
            }
        }

        //[HttpGet("getReply")]
        //public async Task<IActionResult> GetReplies()
        //{
        //    var comment = _mapper.Map<List<CommentDTO>>(_commentService.GetComments());
        //    for (int i = 0;i< comment.Count; i++)
        //    {
        //        if (comment[i].ParentCommentId != null)
        //        {
        //            var replies = new List<CommentDTO>();
        //            replies.Add(comment[i]);
        //            return Ok(replies);
        //        }
        //    }
        //    return Ok("");
        //}

        [HttpPost("createComment")]
        public async Task<ActionResult<Comment>> PostComment(CommentDTO commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);
            comment.Id = Guid.NewGuid();
            comment.ParentCommentId = null;
            comment.CreateOn = DateTime.Now;
            var result = await _commentService.PostComment(comment);
            if (!result)
            {
                return BadRequest("Faild to add comment");
            }
            else
            {
                // notification
                var article = _articleService.GetArticleByID(commentDto.ArticleId); //tìm article
                var user = await _userManager.FindByIdAsync(commentDto.UserId);
                var author = await _userManager.FindByIdAsync(article.StudentId); //tìm thg tác giả của article
                var sampleMessage = "other have commented on your post";
                // delete old noti
                var oldNoti = await _notificationService.GetNotiByUserAndArticle(article.StudentId, commentDto.ArticleId, sampleMessage);
                if (oldNoti != null)
                {
                    var deleteNoti = await _notificationService.DeleteNoti(oldNoti);
                    if (!deleteNoti)
                    {
                        return BadRequest("Error");
                    }
                }
                // create new noti
                var commentCount = await _commentService.GetCommentsCount(commentDto.ArticleId) - 1;
                var message = "";
                if (commentDto.IsAnonymous == false)
                {
                    message = user.FirstName + " " + user.LastName + " and " + commentCount + " other have commented on your post";
                }
                if (commentDto.IsAnonymous == true)
                {
                    message = " An annoynimous user and " + commentCount + " other have commented on your post";
                }
                var notification = new Notification
                {
                    UserId = article.StudentId,
                    UserInteractionId = commentDto.UserId,
                    ArticleId = commentDto.ArticleId,
                    Message = message,
                    IsAnonymous = commentDto.IsAnonymous,
                };
                await _notificationService.PostNotification(notification);
                var email = new EmailDTO
                {
                    Email = author.Email,
                    Subject = "Some one liked your post!",
                    Body = message,
                };
                _emailService.SendEmail(email);
                // dữ liệu trả về khi post
                var commentResult =  _commentService.GetCommentById(comment.Id);

                var commentResponse = _mapper.Map<CommentResponse>(commentResult);
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call
                var firstName = user.FirstName;
                var lastName = user.LastName;
                var userIden = user.Id;
                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712937035/ke2iqrl0rqnxozhxp378.png";
                    cloudUserImage = defaultImageFileName;
                }
                if (commentDto.IsAnonymous == true)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                    firstName = "Anonymous";
                    lastName = "";
                    userIden = "";
                }
                UserComment userComment = new UserComment
                {
                    Id = userIden,
                    UserAvatar = cloudUserImage,
                    FirstName = firstName,
                    LastName = lastName,
                };
                commentResponse.UserComment = userComment;
				return Ok(commentResponse);
            }
        }
        [HttpPost("createReply")]
        public async Task<ActionResult<Comment>> PostReply(Guid parentCommentId , CommentDTO commentDto)
        {
            var parentComment = _commentService.GetCommentById(parentCommentId);
            if (parentComment == null)
            {
                return BadRequest("Not found");
            }
            var reply = _mapper.Map<Comment>(commentDto);
            reply.Id = Guid.NewGuid();
            reply.ParentCommentId = parentCommentId;
            reply.CreateOn = DateTime.Now;

            var result = await _commentService.PostComment(reply);
            if (!result)
            {
                return BadRequest("Faild to add comment");
            }
            else
            {
                //noti
				var user = await _userManager.FindByIdAsync(commentDto.UserId);
                var author = await _userManager.FindByIdAsync(parentComment.UserId); //tìm thg tác giả của article
                var sampleMessage = "other have replied your comment";
                var oldNoti = await _notificationService.GetNotiByUserAndComment(parentComment.UserId, parentCommentId, sampleMessage);
                if (oldNoti != null)
                {
                    var deleteNoti = await _notificationService.DeleteNoti(oldNoti);
                    if (!deleteNoti)
                    {
                        return BadRequest("Error");
                    }
                }

                var repliesCount = await _commentService.GetRepliesCount(parentCommentId) - 1;
                var message = "";
                if (commentDto.IsAnonymous == false)
                {
                    message = user.FirstName + " " + user.LastName + " and " + repliesCount + " other have commented on your post";
                }
                if (commentDto.IsAnonymous == true)
                {
                    message = " An annoynimous user and " + repliesCount + " other have commented on your post";
                }
                var notification = new Notification
                {
                    UserId = parentComment.UserId,
                    UserInteractionId = commentDto.UserId,
                    CommentId = parentCommentId,
                    Message = message,
                    IsAnonymous = commentDto.IsAnonymous,
                };
                await _notificationService.PostNotification(notification);
                var email = new EmailDTO
                {
                    Email = author.Email,
                    Subject = "Some one replied to your comment!",
                    Body = message,
                };
                _emailService.SendEmail(email);
                // dữ liệu trả về
                var replyResult = _commentService.GetCommentById(reply.Id);
                var replyResponse = _mapper.Map<CommentResponse>(replyResult);
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call
                var firstName = user.FirstName;
                var lastName = user.LastName;
                var userIden = user.Id;
                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                if (commentDto.IsAnonymous == true)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                    firstName = "Anonymous";
                    lastName = "";
                    userIden = "";
                }
                UserComment userComment = new UserComment
                {
                    Id = userIden,
                    UserAvatar = cloudUserImage,
                    FirstName = firstName,
                    LastName = lastName,
                };
                replyResponse.UserComment = userComment;
                return Ok(replyResponse);
            }
        }

        [HttpPut()]
        public async Task<ActionResult<Comment>> PutComment(Guid id, string content)
        {
            var comment = _commentService.GetCommentById(id);
            if (comment == null)
            {
                return BadRequest("Not found any comment");
            }
            comment.Content = content;
            //var parentComment = await _context.Comments.FindAsync(id);
            //comment.ParentCommentId = parentComment.ParentCommentId;
            var result = await _commentService.EditComment(comment);
            if (!result)
            {
                return BadRequest("Edit failed");
            }
            else
            {
				var commentResult = _commentService.GetCommentById(id);
                var user = await _userManager.FindByIdAsync(commentResult.UserId);
                var commentResponse = _mapper.Map<CommentResponse>(commentResult);
                var cloudUserImage = await _userService.GetCloudinaryAvatarImagePath(user.Id); // Await the method call
                var firstName = user.FirstName;
                var lastName = user.LastName;
                var userIden = user.Id;
                // If imageBytes is null, read the default image file
                if (cloudUserImage == null)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                }
                if (comment.IsAnonymous == true)
                {
                    var defaultImageFileName = "http://res.cloudinary.com/dizeyf6y0/image/upload/v1712075739/pxfrfocprhnsriutmg3r.jpg";
                    cloudUserImage = defaultImageFileName;
                    firstName = "Anonymous";
                    lastName = "";
                    userIden = "";
                }
                UserComment userComment = new UserComment
                {
                    Id = userIden,
                    UserAvatar = cloudUserImage,
                    FirstName = firstName,
					LastName = lastName
                };
                commentResponse.UserComment = userComment;
                return Ok(commentResponse);
            }
        }


        [HttpDelete()]
        public async Task<ActionResult> DeleteComment(Guid commentId)
        {
            //hàm đệ quy để xoá comment và tất cả các reply
            async Task DeleteCommentAndReplies(Guid id)
            {
                var replies = await _commentService.GetReplies(id);

                foreach (var reply in replies)
                {
                    await DeleteCommentAndReplies(reply.Id); //gọi đệ quy
                    //delete likes, dislike va noti của reply
                    var likesOfReply = await _likeService.GetCommentLikes(reply.Id);
                    var dislikesOfReply = await _dislikeService.GetCommentDislikes(reply.Id);
                    var noti = await _notificationService.GetNotiByComment(reply.Id);
                    _context.Notifications.RemoveRange(noti);
                    _context.Likes.RemoveRange(likesOfReply); 
                    _context.Dislikes.RemoveRange(dislikesOfReply);
                    //delete reply
                    _context.Comments.Remove(reply); 
                }
            }

            //delete likes và dislike của comment gốc
            var likesOfComment = await _likeService.GetCommentLikes(commentId);
            var dislikesOfComment = await _dislikeService.GetCommentDislikes(commentId);
            var noti = await _notificationService.GetNotiByComment(commentId);
            _context.RemoveRange(noti);
            _context.Likes.RemoveRange(likesOfComment);
            _context.Dislikes.RemoveRange(dislikesOfComment);


            //xoá comment gốc
            var commentToDelete = _commentService.GetCommentById(commentId);

           
            if (commentToDelete.ParentCommentId != null)
            {
                var parentComment = _commentService.GetCommentById((Guid)commentToDelete.ParentCommentId);
                var commentResponse = _mapper.Map<CommentResponse>(parentComment);
                commentResponse.hasReplies = await _commentService.HasReplies(parentComment.Id);
                return Ok(commentResponse);
            }
            

            if (commentToDelete != null)
            {
                await DeleteCommentAndReplies(commentId); //xoá tất cả replies, likes và dislikes liên quan
                _context.Comments.Remove(commentToDelete); //xoá comment gốc
            }
            var result = await _commentService.Save(); //save vào db

          
            if (!result)
            {
                return BadRequest("Failed");
            }
            return Ok("Delete successful");

        }

    }
}
