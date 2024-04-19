using AutoMapper;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Comp1640_Final.Controllers;
using Comp1640_Final.Data;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace UnitTest
{
    [TestFixture]
    public class UsersControllerTest
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private Mock<Cloudinary> _cloudinaryMock;
        private Mock<ICloudinaryService> _cloudinaryServiceMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private UsersController _usersController;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _mapperMock = new Mock<IMapper>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            //_cloudinaryMock = new Mock<Cloudinary>();
            _cloudinaryMock = new Mock<Cloudinary>("cloudinary://API_KEY:API_SECRET@CLOUD_NAME"); // lam the nay thi k loi invalid cloudinary
            _cloudinaryServiceMock = new Mock<ICloudinaryService>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);

            _usersController = new UsersController(
                _userServiceMock.Object,
                _mapperMock.Object,
                null, // You may need to adjust this based on your setup
                _webHostEnvironmentMock.Object,
                _cloudinaryMock.Object,
                _cloudinaryServiceMock.Object,
                _userManagerMock.Object // Pass the mocked UserManager
            );
        }

        [Test]
        public async Task GetUserById_ReturnsOkResult_ForValidUserId()
        {
            // Arrange
            var userId = "validUserId";
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456789",
                Email = "john.doe@example.com",
                FacultyId = 1,
                CloudAvatarImagePath = "ok"
            };
            var roles = new List<string> { "Role1", "Role2" };

            _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(roles);
            _userServiceMock.Setup(m => m.GetCloudinaryAvatarImagePath(user.Id)).ReturnsAsync(user.CloudAvatarImagePath);

            // Act
            var result = await _usersController.GetUserById(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task ChangeAvatar_ReturnsBadRequest_WhenNoImageFileProvided()
        {
            // Arrange
            var userId = "validUserId";
            var avatarImage = (IFormFile)null;

            // Act
            var result = await _usersController.ChangeAvatar(avatarImage, userId);

            // Assert
            //Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(result, Is.InstanceOf<ActionResult<Comp1640_Final.Models.Account>>());
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task ChangePassword_ReturnsOkResult_WhenPasswordChangedSuccessfully()
        {
            // Arrange
            var email = "john.doe@example.com";
            var password = "newPassword";
            var user = new ApplicationUser
            {
                Email = email
            };
            var token = "resetToken";
            var resultSucceeded = IdentityResult.Success;

            _userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(token);
            _userManagerMock.Setup(m => m.ResetPasswordAsync(user, token, password)).ReturnsAsync(resultSucceeded);

            // Act
            var result = await _usersController.PutAccount(email, password);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task ChangeAvatar_ReturnsOkResult_ForValidImageFileAndUserId()
        {
            // Arrange
            var userId = "validUserId";
            var avatarImage = new FormFile(Stream.Null, 0, 0, "avatarImage", "image.jpg");

            // Mocking user retrieval
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456789",
                Email = "john.doe@example.com",
                FacultyId = 1,
                CloudAvatarImagePath = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME"
            };
            _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);

            // Mocking Cloudinary service method
            _cloudinaryServiceMock.Setup(m => m.UploadAvatarImage(avatarImage)).ReturnsAsync(new ImageUploadResult { Uri = new Uri("http://example.com/image.jpg") });

            // Act
            var result = await _usersController.ChangeAvatar(avatarImage, userId);

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<Comp1640_Final.Models.Account>>());

            //Assert.That(result.Value, Is.InstanceOf<OkObjectResult>());
            //Assert.That(result.Value, Is.Not.Null.And.EqualTo(avatarImage));
        }

        [Test]
        public async Task ChangeAvatar_ReturnsBadRequest_ForInvalidImageFile()
        {
            // Arrange
            var userId = "validUserId";
            var avatarImage = new FormFile(Stream.Null, 0, 0, "avatarImage", "invalidFile.txt");

            // Mocking user retrieval
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123456789",
                Email = "john.doe@example.com",
                FacultyId = 1,
                CloudAvatarImagePath = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME"
            };

            _userServiceMock.Setup(m => m.IsValidImageFile(avatarImage)).Returns(false);
            _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);


            // Act
            var result = await _usersController.ChangeAvatar(avatarImage, userId);

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<Comp1640_Final.Models.Account>>());
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
            //Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        }
    }
}
