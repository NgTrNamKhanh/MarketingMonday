//using AutoMapper;
//using CloudinaryDotNet.Actions;
//using CloudinaryDotNet;
//using Comp1640_Final.Controllers;
//using Comp1640_Final.Data;
//using Comp1640_Final.Models;
//using Comp1640_Final.Services;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace UnitTest
//{
//    [TestFixture]
//    public class UsersControllerTest
//    {
//        private UsersController _usersController;
//        private Mock<IUserService> _userServiceMock;
//        private Mock<IMapper> _mapperMock;
//        private Mock<ProjectDbContext> _contextMock;
//        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
//        private Mock<Cloudinary> _cloudinaryMock;
//        private Mock<UserManager<ApplicationUser>> _userManagerMock;

//        [SetUp]
//        public void Setup()
//        {
//            _userServiceMock = new Mock<IUserService>();
//            _mapperMock = new Mock<IMapper>();
//            //_contextMock = new Mock<ProjectDbContext>();
//            _contextMock = new Mock<ProjectDbContext>();
//            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
//            _cloudinaryMock = new Mock<Cloudinary>();
//            _userManagerMock = MockUserManager<ApplicationUser>();

//            _usersController = new UsersController(
//                _userServiceMock.Object,
//                _mapperMock.Object,
//                _contextMock.Object,
//                _webHostEnvironmentMock.Object,
//                _cloudinaryMock.Object,
//                _userManagerMock.Object
//            );
//        }

//        [Test]
//        public async Task GetUserById_ReturnsOkResult_WithUserDto()
//        {
//            // Arrange
//            var userId = "validUserId";
//            var user = new ApplicationUser
//            {
//                Id = userId,
//                FirstName = "John",
//                LastName = "Doe",
//                PhoneNumber = "123456789",
//                Email = "john.doe@example.com",
//                FacultyId = 3,
//                CloudAvatarImagePath = "avatarImagePath"
//            };
//            var roles = new List<string> { "Admin" };
//            var accountDto = new
//            {
//                Id = user.Id,
//                FirstName = user.FirstName,
//                LastName = user.LastName,
//                PhoneNumber = user.PhoneNumber,
//                Email = user.Email,
//                Role = roles,
//                FacultyId = user.FacultyId,
//                CloudAvatar = user.CloudAvatarImagePath
//            };

//            _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);
//            _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(roles);
//            _userServiceMock.Setup(x => x.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(user.CloudAvatarImagePath);

//            // Act
//            var result = await _usersController.GetUserById(userId);

//            // Assert
//            //Assert.That(result, Is.Not.Null.And.InstanceOf<OkObjectResult>());
//            //var okResult = result as OkObjectResult;
//            //Assert.That(okResult.Value, Is.Not.Null.And.EqualTo(accountDto));

//            // Assert
//            Assert.That(result, Is.Not.Null.And.InstanceOf<ActionResult<IEnumerable<ApplicationUser>>>());
//            var actionResult = result as ActionResult<IEnumerable<ApplicationUser>>;
//            Assert.That(actionResult.Result, Is.InstanceOf<OkObjectResult>());
//            var okResult = actionResult.Result as OkObjectResult;


//            //Assert.That(result, Is.Not.Null.And.InstanceOf<OkObjectResult>());
//            //var okResult = result as OkObjectResult;
//            //Assert.That(okResult.Value, Is.EqualTo(role.RoleName));
//        }

//        // Add more test methods for other controller actions as needed

//        private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
//        {
//            var store = new Mock<IUserStore<TUser>>();
//            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
//        }
//    }
//}
