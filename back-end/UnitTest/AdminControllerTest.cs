using AutoMapper;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Comp1640_Final.Controllers;
using Comp1640_Final.IServices;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace UnitTest
{
    [TestFixture]
    public class AdminControllerTest
    {
        private AdminController _adminController;
        private Mock<IAuthService> _authServiceMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<Cloudinary> _cloudinaryMock;

        [SetUp]
        public void Setup()
        {
            _authServiceMock = new Mock<IAuthService>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>();
            _userServiceMock = new Mock<IUserService>();
            _cloudinaryMock = new Mock<Cloudinary>();

            _adminController = new AdminController(
                _authServiceMock.Object,
                _userManagerMock.Object,
                _userServiceMock.Object,
                _cloudinaryMock.Object);
        }

        [Test]
        public async Task CreateAccount_Test()
        {
            // Arrange
            var accountDto = new Comp1640_Final.Models.Account
            {
                FirstName = "Quoc",
                LastName = "Viet",
                PhoneNumber = "1234567890",
                Email = "viet@gmail.com",
                Password = "12345",
                Role = "Admin",
                FacultyId = 1
                
            };
            //var user = new ApplicationUser { /* Populate ApplicationUser mock object if needed */ };

           
            _authServiceMock.Setup(a => a.CreateAccountUser(accountDto)).ReturnsAsync(true);
            //_cloudinaryMock.Setup(c => c.UploadAsync(It.IsAny<ImageUploadParams>())).ReturnsAsync(new ImageUploadResult());

            // Act
            var result = await _adminController.CreateAccount(accountDto) ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(result, Is.EqualTo("Create Successful"));
        }

    }
}
