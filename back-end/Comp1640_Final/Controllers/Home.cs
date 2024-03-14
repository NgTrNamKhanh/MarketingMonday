using AutoMapper;
using Comp1640_Final.DTO;
using Comp1640_Final.IServices;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Comp1640_Final.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public HomeController(IAuthService authService, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _authService = authService;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string passWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _authService.Login(email, passWord))
            {
                var identityUser = await _userManager.FindByEmailAsync(email);
                var roles = await _userManager.GetRolesAsync(identityUser);

                var accountDto = new LoginDTO
                {
                    Id = identityUser.Id,
                    FirstName = identityUser.FirstName,
                    LastName = identityUser.LastName,
                    PhoneNumber = identityUser.PhoneNumber,
                    Email = identityUser.Email,
                    Roles = roles.ToArray(),
                    FacultyId = identityUser.FacultyId
                };

                return Ok(accountDto);
            }

            return BadRequest();
        }

    }
}
