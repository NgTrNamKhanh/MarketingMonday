using Comp1640_Final.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [HttpPost("CreateRole")]
        public async Task<IActionResult> PostRole(Role role)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(role.RoleName));
            if (result.Succeeded)
            {
                return Ok(role.RoleName);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetRole()
        {
            return await _roleManager.Roles.ToListAsync();
        }
    }
}
