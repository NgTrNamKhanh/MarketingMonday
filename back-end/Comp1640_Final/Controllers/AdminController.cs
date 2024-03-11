using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comp1640_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
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

        //[HttpPut]
        //public async Task<IActionResult> PutAccount(string email, string password)
        //{
        //    //name = account.Email;
        //    var user = await _userManager.FindByEmailAsync(email);
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var result = await _userManager.ResetPasswordAsync(user, token, password);
        //    if (result.Succeeded)
        //    {
        //        return Ok("Successful");
        //    }
        //    return BadRequest("Failed");
        //}

        [HttpPut]
        public async Task<IActionResult> PutAccountForAdmin(string username, Account account)
        {
            //name = account.Email;
            var user = await _userManager.FindByEmailAsync(username);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, account.Password);
            user.Email = account.Email;
            user.UserName = account.Email;
            user.FirstName = account.FirstName;
            user.LastName = account.LastName;
            var changeEmail = await _userManager.UpdateAsync(user);
            if (result.Succeeded && changeEmail.Succeeded)
            {
                return Ok("Successful");
            }
            return BadRequest("Failed");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(); // Người dùng không tồn tại
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(); // Xóa thành công
            }
            else
            {
                return BadRequest(result.Errors); // Xử lý lỗi
            }
        }


    }
}
