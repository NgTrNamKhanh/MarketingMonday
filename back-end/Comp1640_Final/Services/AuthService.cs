using Comp1640_Final.IServices;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Comp1640_Final.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> CreateAccountUser(Account account)
        {
            if (string.IsNullOrEmpty(account.FirstName) ||
                string.IsNullOrEmpty(account.LastName) ||
                string.IsNullOrEmpty(account.Email) ||
                string.IsNullOrWhiteSpace(account.Password))
            {
                return false;
            }
            var identityUser = new ApplicationUser
            {
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                UserName = account.Email,
                FacultyId = account.FacultyId,
                PhoneNumber = account.PhoneNumber,
                CloudAvatarImagePath = account.CloudAvatarImagePath,
            };
            var result = await _userManager.CreateAsync(identityUser, account.Password);
            var role = await _userManager.AddToRoleAsync(identityUser, account.Role);
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Error: {error.Description}");
            }
            return result.Succeeded && role.Succeeded;
        }

        public async Task<bool> EditAccount(EditAccountDTO account, string userId)
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
                    return false;
                }
            }

            user.Email = account.Email;
            user.PhoneNumber = account.PhoneNumber;
            user.UserName = account.Email;
            user.FirstName = account.FirstName;
            user.LastName = account.LastName;
            user.FacultyId = account.FacultyId;
            await _userManager.RemoveFromRolesAsync(user, oldRoles);
            // Thêm vai trò mới
            var changeRole = await _userManager.AddToRoleAsync(user, account.Role);
            var changeEmail = await _userManager.UpdateAsync(user);

            return changeRole.Succeeded && changeEmail.Succeeded;
        }

        public  IEnumerable<ApplicationUser> GetAllUsers()
        {
            return  _userManager.Users.ToList();
        }

        public async Task<bool> Login(string email, string passWord)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);
            if (identityUser == null)
            {
                return false;
            }
            return await _userManager.CheckPasswordAsync(identityUser, passWord);
        }

        

    }
}
