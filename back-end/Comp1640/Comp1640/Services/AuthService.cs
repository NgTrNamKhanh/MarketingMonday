using Comp1640.Models;
using Microsoft.AspNetCore.Identity;

namespace Comp1640.Services
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

            };
            var result = await _userManager.CreateAsync(identityUser, account.Password);
            return result.Succeeded;
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
