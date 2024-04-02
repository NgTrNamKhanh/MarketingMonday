using Comp1640_Final.IServices;
using Comp1640_Final.Models;
using Microsoft.AspNetCore.Identity;
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
