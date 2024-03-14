using AutoMapper;
using Comp1640_Final.DTO;
using Comp1640_Final.IServices;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Comp1640_Final.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;


        public AdminController(IAuthService authService, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _authService = authService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("createAccount")]
        public async Task<IActionResult> CreateAccount(Account account)
        {
            if (await _authService.CreateAccountUser(account))
            {
                return Ok("Create Successful");
            }
            return BadRequest("Something went wrong");
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login(string email, string passWord)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (await _authService.Login(email, passWord))
        //    {
        //        return Ok(email +"\n" + passWord);
        //    }
        //    return BadRequest();
        //}

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

        [HttpPut("account")]
        public async Task<IActionResult> PutAccountForAdmin(string userId, Account account)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var oldRoles = await _userManager.GetRolesAsync(user);
            if (!string.IsNullOrWhiteSpace(account.Password))
            {
                // If password is provided and not empty, reset it
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, account.Password);
                if (!result.Succeeded)
                {
                    return BadRequest("Failed to reset password");
                }
            }
            user.Email = account.Email;
            user.PhoneNumber = account.PhoneNumber;
            user.UserName = account.Email;
            user.FirstName = account.FirstName;
            user.LastName = account.LastName;
            await _userManager.RemoveFromRolesAsync(user, oldRoles);
            // Thêm vai trò mới
            var changeRole = await _userManager.AddToRoleAsync(user, account.Role);
            var changeEmail = await _userManager.UpdateAsync(user);
            if (changeEmail.Succeeded)
            {
                return Ok("Successful");
            }
            return BadRequest("Failed");
        }

        [HttpGet("accounts")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {

            var users = await _userManager.Users.ToListAsync();
            var accountDto = new List<object>();
               // _mapper.Map<List<AccountDTO>>(await _userManager.Users.ToListAsync()) ;
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                accountDto.Add(new
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Role = roles,
                    FacultyId = user.FacultyId
                });
            }
            return Ok(accountDto);
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
