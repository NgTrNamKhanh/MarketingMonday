using Comp1640.Models;
using Comp1640.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Comp1640.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AdminController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount(Account account)
        {
            if (await _authService.CreateAccountUser(account))
            {
                return Ok("Create Successful");
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string passWord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _authService.Login(email, passWord))
            {
                return Ok("Done");
            }
            return BadRequest();
        }
    }
}
