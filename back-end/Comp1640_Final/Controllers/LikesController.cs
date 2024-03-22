using AutoMapper;
using Comp1640_Final.Data;
using Comp1640_Final.DTO.Request;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Comp1640_Final.Controllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ProjectDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILikeService _likeService;
        private readonly IAritcleService _aritcleService;

        public LikesController(ProjectDbContext context, IMapper mapper, ILikeService likeService, IAritcleService aritcleService)
        {
            _context = context;
            _mapper = mapper;
            _likeService = likeService;
            _aritcleService = aritcleService;
        }

        [HttpGet("{articleId}")]
        public async Task<IActionResult> GetLikes(Guid articleId)
        {
            if (!_aritcleService.ArticleExists(articleId))
            {
                return NotFound();
            }
            var likes = await _likeService.GetLikes(articleId);
            var likeResponse = _mapper.Map<List<InteractResponse>>(likes);
            return Ok(likeResponse);
        }

        [HttpPost]
        public async Task<ActionResult<Like>> PostLike([FromForm]InteractDTO likeDto)
        {
            var like = _mapper.Map<Like>(likeDto);
            likeDto.Id = null;
            like.Date = DateTime.Now;
            var result = await _likeService.PostLike(like);
            if (!result)
            {
                return BadRequest("Error");
            }else
            {
                return Ok("Successful");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            var like = await _context.Likes.FindAsync(id);
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
    }
}
