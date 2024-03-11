using Comp1640_Final.Models;

namespace Comp1640_Final.Services
{
    public interface IAuthService
    {
        Task<bool> CreateAccountUser(Account account);
        Task<bool> Login(string email, string passWord);
    }
}