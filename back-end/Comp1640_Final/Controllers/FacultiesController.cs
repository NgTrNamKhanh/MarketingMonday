using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultiesController : ControllerBase
    {
        public readonly ProjectDbContext _context;

        public FacultiesController(ProjectDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Faculty>>> GetFalcuties()
        {
            return await _context.Faculties.ToListAsync();
        }
    }
}
