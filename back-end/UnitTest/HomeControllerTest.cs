using AutoMapper;
using Comp1640_Final.Controllers;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.IServices;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class HomeControllerTest
    {
        private Mock<IAuthService> _authServiceMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IUserService> _userServiceMock;
        private HomeController _homeController;

        [SetUp]
        public void Setup()
        {
            _authServiceMock = new Mock<IAuthService>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _configurationMock = new Mock<IConfiguration>();
            _userServiceMock = new Mock<IUserService>();

            _homeController = new HomeController(
                _authServiceMock.Object,
                _userManagerMock.Object,
                null, // Replace null with a mocked IMapper if needed
                _configurationMock.Object,
                null, // Replace null with a mocked IWebHostEnvironment if needed
                _userServiceMock.Object
            );
        }

        //[Test]
        //public async Task Login_ReturnsOkResult_WithValidCredentials()
        //{
        //    // Arrange
        //    var email = "test@example.com";
        //    var password = "password";
        //    var user = new ApplicationUser
        //    {
        //        Id = "validUserId",
        //        FirstName = "John",
        //        LastName = "Doe",
        //        PhoneNumber = "123456789",
        //        Email = email,
        //        FacultyId = 1
        //    };
        //    var roles = new List<string> { "Role1", "Role2" };

        //    _authServiceMock.Setup(m => m.Login(email, password)).ReturnsAsync(true);
        //    _userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync(user);
        //    _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(roles);
        //    _userServiceMock.Setup(m => m.GetCloudinaryAvatarImagePath(user.Id)).ReturnsAsync("ok");

        //    // Mock token creation
        //    _configurationMock.Setup(m => m.GetSection("Jwt:Key").Value).Returns("your_secret_key");
        //    _configurationMock.Setup(m => m.GetSection("Jwt:Issuer").Value).Returns("your_issuer");
        //    _configurationMock.Setup(m => m.GetSection("Jwt:Audience").Value).Returns("your_audience");

        //    // Act
        //    var result = await _homeController.Login(email, password);

        //    // Assert
        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result, Is.InstanceOf<OkObjectResult>());

        //    var okResult = result as OkObjectResult;
        //    Assert.That(okResult, Is.Not.Null);

        //    var value = okResult.Value as LoginResponse;
        //    Assert.That(value, Is.Not.Null);

        //    Assert.That(value.Email, Is.EqualTo(email));
        //    Assert.That(value.Roles, Is.EqualTo(roles));
        //    Assert.That(value.Id, Is.EqualTo(user.Id));
        //    // Add more assertions as needed
        //}

        [Test]
        public async Task Login_ReturnsBadRequest_WithInvalidCredentials()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password";

            _authServiceMock.Setup(m => m.Login(email, password)).ReturnsAsync(false);

            // Act
            var result = await _homeController.Login(email, password);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            // Add more assertions as needed
        }

        // Helper method to mock UserManager<ApplicationUser>
        //private Mock<UserManager<ApplicationUser>> MockUserManager<T>() where T : IdentityUser
        //{
        //    var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        //    return new Mock<UserManager<ApplicationUser>>(
        //        userStoreMock.Object, null, null, null, null, null, null, null, null);
        //}
    }
}
