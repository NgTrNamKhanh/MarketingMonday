using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestFixture]
    public class AuthServiceTest
    {
        private AuthService _authService;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;

        [SetUp] 
        public void SetUp()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _authService = new AuthService(_userManagerMock.Object);
        }

        [Test]
        public async Task CreateAccount_Successful_Test()
        {
            
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success); //giả định cho method CreateAsync
            _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success); //giả định cho mthod AddToRoleAsync

            var account = new Account
            {
                FirstName = "Quoc",
                LastName = "Viet",
                PhoneNumber = "1234567890",
                Email = "viet@gmail.com",
                Password = "Viet123",
                Role = "Admin",
                FacultyId = 1
            };

            var result = await _authService.CreateAccountUser(account);

            //assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task CreateAccount_Failed_Test()
        {

            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var account = new Account
            {
                FirstName = null,
                LastName = "Viet",
                PhoneNumber = "1234567890",
                Email = null,
                Password = "Viet123",
                Role = "Admin",
                FacultyId = 1
            };

            var result = await _authService.CreateAccountUser(account);

            //assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task EditAccount_Test()
        {
            var account = new EditAccountDTO
            {
                FirstName = "Quoc",
                LastName = "Viet",
                PhoneNumber = "1234567890",
                Email = "viet@gmail.com",
                Password = "Viet123",
                Role = "Admin",
                FacultyId = 1
            };
            var userId = "user123";

            var user = new ApplicationUser
            {
                Id = userId,
                
            };
            var oldRoles = new List<string> { "OldRole" };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user); //test xem ton tai user khong
            _userManagerMock.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(oldRoles); //test get role user
            _userManagerMock.Setup(u => u.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("resetToken"); //token
            _userManagerMock.Setup(u => u.ResetPasswordAsync(user, "resetToken", account.Password)).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.RemoveFromRolesAsync(user, oldRoles)).ReturnsAsync(IdentityResult.Success); //test remove role cũ
            _userManagerMock.Setup(u => u.AddToRoleAsync(user, account.Role)).ReturnsAsync(IdentityResult.Success); // test add role mới

            var result = await _authService.EditAccount(account, userId);

            //assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task Login_CorrectPasswordAndEmail_Test()
        {

            var email = "abc@gmail.com";
            var password = "pw1234";
            var user = new ApplicationUser
            {
                Email = email,
            };
            _userManagerMock.Setup(u => u.FindByEmailAsync(email)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, password)).ReturnsAsync(true);//giả sử đúng mật khẩu
            var result = await _authService.Login(email, password);

            //assert
            Assert.That(result, Is.True);
        }


        [Test]
        public async Task Login_WrongPassword_Test()
        {
            var email = "abc@gmail.com";
            var password = "pw1234";
            var user = new ApplicationUser
            {
                Email = email,
            };
            _userManagerMock.Setup(u => u.FindByEmailAsync(email)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, password)).ReturnsAsync(false);//giả sử sai mật khẩu
            var result = await _authService.Login(email, password);

            //assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task Login_EmailNotExist_Test()
        {
            var email = "abc@gmail.com";
            var password = "pw1234";

            _userManagerMock.Setup(u => u.FindByEmailAsync(email)).ReturnsAsync((ApplicationUser)null); //gỉa sử sai email
            var result = await _authService.Login(email, password);

            //assert
            Assert.That(result, Is.False);
        }
       

    }
}
