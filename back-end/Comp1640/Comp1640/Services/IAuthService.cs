using Comp1640.Models;

namespace Comp1640.Services
{
    public interface IAuthService
    {
        Task<bool> CreateAccountUser(Account account);
        Task<bool> Login(string email, string passWord);
    }
}