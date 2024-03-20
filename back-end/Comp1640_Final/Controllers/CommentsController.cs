using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO.Request;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Migrations;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
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

        public CommentsController(ProjectDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, ICommentService commentService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _commentService = commentService;
        }

        [HttpGet("getComment")]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _commentService.GetComments();
            if (comments == null)
            {
                return NotFound();
            }
            else
            {
                List<CommentResponse> commentResponses = new List<CommentResponse>();
                foreach (var comment in comments)
                {
                    var commentResponse = _mapper.Map<CommentResponse>(comment);

                    var user = await _userManager.FindByIdAsync(comment.UserId);
                    if (user != null)
                    {
                        UserComment userComment = new UserComment
                        {
                            UserId = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName
                        };
                        commentResponse.UserComment = userComment;
                    }

                    commentResponses.Add(commentResponse);
                }

                return Ok(commentResponses);
            }
        }

        [HttpGet("getParentComments")]
        public async Task<IActionResult> GetParentComments(Guid articleId)
        {
            var comments =  await _commentService.GetParentComments(articleId);
            if (comments == null)
            {
                return NotFound();
            }
            else 
            {
                List<CommentResponse> commentResponses = new List<CommentResponse>();
                foreach (var comment in comments) 
                {
                    var commentResponse = _mapper.Map<CommentResponse>(comment);

                    var user = await _userManager.FindByIdAsync(comment.UserId);
                    if (user != null)
                    {
                        UserComment userComment = new UserComment
                        {
                            UserId = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName
                        };
                        commentResponse.UserComment = userComment;
                    }

                    commentResponses.Add(commentResponse);
                }

                return Ok(commentResponses);
            }
        }

        [HttpGet("getReplies")]
        public async Task<IActionResult> GetReplies(Guid parentId)
        {
            var comments = await _commentService.GetReplies(parentId);
            if (comments == null)
            {
                return NotFound();
            }
            else
            {
                List<CommentResponse> commentResponses = new List<CommentResponse>();
                foreach (var comment in comments)
                {
                    var commentResponse = _mapper.Map<CommentResponse>(comment);

                    var user = await _userManager.FindByIdAsync(comment.UserId);
                    if (user != null)
                    {
                        UserComment userComment = new UserComment
                        {
                            UserId = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName
                        };
                        commentResponse.UserComment = userComment;
                    }

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
            var result = await _commentService.PostComment(comment);
            if (!result)
            {
                return BadRequest("Faild to add comment");
            }
            else
            {
                return Ok("Add successful");
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

            var result = await _commentService.PostComment(reply);
            if (!result)
            {
                return BadRequest("Faild to add comment");
            }
            else
            {
                return Ok("Add successful");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Comment>> PutComment(Guid id, CommentDTO commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);
            var parentComment = await _context.Comments.FindAsync(id);
            comment.Id = id;
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
        [HttpDelete("{id}")]
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
    }
}
