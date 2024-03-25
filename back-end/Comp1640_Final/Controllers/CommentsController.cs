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

        public CommentsController(ProjectDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, 
            ICommentService commentService, IWebHostEnvironment webHostEnvironment, IUserService userService, ILikeService likeService, 
            IDislikeService dislikeService, IHttpContextAccessor httpContextAccessor)
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

        }

        [HttpGet("getComment")]
        public async Task<IActionResult> GetComments(string userId)
        {
            var comments = await _commentService.GetComments();
            if (comments == null)
            {
                return Ok(comments);
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
                        var userImageBytes = await _userService.GetImagesByUserId(user.Id); // Await the method call

                        // If imageBytes is null, read the default image file
                        if (userImageBytes == null)
                        {
                            var defaultImageFileName = "default-avatar.jpg";
                            var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                            userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                        }
                        UserComment userComment = new UserComment
                        {
                            UserId = user.Id,
                            UserAvatar = userImageBytes,
                            UserName = user.FirstName + " " + user.LastName,
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

        [HttpGet("getParentComments")]
        public async Task<IActionResult> GetParentComments(Guid articleId, string userId)
        {
            var comments =  await _commentService.GetParentComments(articleId);
            if (comments == null)
            {
                return Ok(comments);
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
                        var userImageBytes = await _userService.GetImagesByUserId(user.Id); // Await the method call

                        // If imageBytes is null, read the default image file
                        if (userImageBytes == null)
                        {
                            var defaultImageFileName = "default-avatar.jpg";
                            var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                            userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                        }
                        UserComment userComment = new UserComment
                        {
                            UserId = user.Id,
                            UserAvatar = userImageBytes,
                            UserName = user.FirstName + " " + user.LastName,
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
            if (comments == null)
            {
                return Ok(comments);
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
                        var userImageBytes = await _userService.GetImagesByUserId(user.Id); // Await the method call

                        // If imageBytes is null, read the default image file
                        if (userImageBytes == null)
                        {
                            var defaultImageFileName = "default-avatar.jpg";
                            var defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserAvatars", "DontHaveAva", defaultImageFileName);
                            userImageBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                        }
                        UserComment userComment = new UserComment
                        {
                            UserId = user.Id,
                            UserAvatar = userImageBytes,
                            UserName = user.FirstName + " " + user.LastName,
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
                var commentResult = _context.Comments.Find(comment.Id);
                var commentResponse = _mapper.Map<CommentResponse>(commentResult);
                return Ok(commentResponse);
            }
        }
        [HttpPost("createReply")]
        public async Task<ActionResult<Comment>> PostReply(Guid parentCommentId , CommentDTO commentDto)
        {
            var parentComment = await _context.Comments.FindAsync(parentCommentId);
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
                var replyResult = _context.Comments.Find(reply.Id);
                var replyResponse = _mapper.Map<CommentResponse>(replyResult);
                return Ok(replyResponse);
            }
        }

        [HttpPut()]
        public async Task<ActionResult<Comment>> PutComment(Guid id, CommentDTO commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);
            var parentComment = await _context.Comments.FindAsync(id);
            //comment.Id = id;
            comment.ParentCommentId = parentComment.ParentCommentId;
            var result = await _commentService.EditComment(comment);
            if (!result)
            {
                return BadRequest("Edit failed");
            }
            else
            {
                return Ok("Edit successful");
            }
        }
        [HttpDelete()]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            if (!_commentService.CommentExists(id))
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            //var comments = _mapper.Map<List<CommentDTO>>(_commentService.GetComments());
            var result = await _commentService.DeleteComment(comment);

            //foreach (CommentDTO c in comments)
            //{
            //    if (c.ParentCommentId == id) 
            //    {
            //        var reply = await _context.Comments.FindAsync(c.Id);
            //        var result1 = await _commentService.DeleteComment(reply);
            //        if (!result1 && !result)
            //        {
            //            return BadRequest("Failed");
            //        }
            //        return Ok("Successful");
            //    }
            //}


            if (!result)
            {
                return BadRequest("Failed");
            }
            return  Ok("Delete successful");
        }
        private async Task<string> GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var user = await _userManager.FindByEmailAsync(principal.Identity.Name);
            if (user != null)
            {
                return user.Id;
            }
            return null;
        }
    }
}
