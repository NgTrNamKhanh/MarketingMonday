using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO;
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
            var comment = _mapper.Map<List<CommentDTO>>(_commentService.GetComments());
            return Ok(comment);
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
            commentDto.Id = Guid.NewGuid();
            commentDto.ParentCommentId = null;
            var comment = _mapper.Map<Comment>(commentDto);
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
            commentDto.Id= Guid.NewGuid();
            commentDto.ParentCommentId = parentCommentId;
            var reply = _mapper.Map<Comment>(commentDto);

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
