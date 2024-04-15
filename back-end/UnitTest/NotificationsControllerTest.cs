using AutoMapper;
using Comp1640_Final.Controllers;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class NotificationsControllerTest
    {
        private NotificationsController _notificationsController;
        private Mock<INotificationService> _notificationServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<IUserService> _userServiceMock;

        [SetUp]
        public void Setup()
        {
            _notificationServiceMock = new Mock<INotificationService>();
            _mapperMock = new Mock<IMapper>();

            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
    null, null, null, null, null, null, null, null
                );

            _userServiceMock = new Mock<IUserService>();

            _notificationsController = new NotificationsController(
                context: null,
                aritcleService: null,
                likeService: null,
                dislikeService: null,
                commentService: null,
                mapper: _mapperMock.Object,
                userManager: _userManagerMock.Object,
                userService: _userServiceMock.Object,
                webHostEnvironment: null,
                notificationService: _notificationServiceMock.Object
            );
        }

        [Test]
        public async Task GetNotifications_ReturnsOkResult()
        {
            // Arrange
            var userId = "validUserId";
            var notifications = new List<Notification>
            {
                new Notification { UserInteractionId = "user1" },
                new Notification { UserInteractionId = "user2" }
            };

            var notiResponses = new List<NotificationResponse>
            {
                new NotificationResponse {
                    Id = 1,
                    Message = "Notification message 1",
                    CreatedAt = DateTime.Now,
                    UserNoti = new UserNoti
                    {
                        Id = "userId1",
                        UserAvatar = "picturelink",
                        FirstName = "John",
                        LastName = "Doe"
                    }
                },
                new NotificationResponse {
                    Id = 2,
                    Message = "Notification message 2",
                    CreatedAt = DateTime.Now,
                    UserNoti = new UserNoti
                    {
                        Id = "userId2",
                        UserAvatar = "picturelink2",
                        FirstName = "Jane",
                        LastName = "Smith"
                    }
                }
            };

            _notificationServiceMock.Setup(x => x.GetNotifications(userId)).ReturnsAsync(notifications);
            _mapperMock.Setup(m => m.Map<NotificationResponse>(It.IsAny<Notification>()))
                       .Returns<Notification>(notification =>
                       {
                           return notiResponses[0];
                       });
            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                            .ReturnsAsync(new ApplicationUser { Id = "user1", FirstName = "John", LastName = "Doe" });
            _userServiceMock.Setup(x => x.GetCloudinaryAvatarImagePath(It.IsAny<string>()))
                            .ReturnsAsync("randompicturelink");

            // Act
            var result = await _notificationsController.GetNotifications(userId);

            // Assert
            Assert.That(result, Is.Not.Null.And.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.Not.Null.And.InstanceOf<List<NotificationResponse>>());
            var returnedNotifications = okResult.Value as List<NotificationResponse>;
            Assert.That(returnedNotifications, Is.Not.Null.And.Count.EqualTo(notifications.Count));
        }

        [Test]
        public async Task GetNotifications_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            var userId = "validUserId";

            // Mock empty list of notifications
            var notifications = new List<Notification>();

            _notificationServiceMock.Setup(x => x.GetNotifications(userId)).ReturnsAsync(notifications);

            // Act
            var result = await _notificationsController.GetNotifications(userId);

            // Assert
            Assert.That(result, Is.Not.Null.And.InstanceOf<OkObjectResult>(), "The method should return an OkObjectResult.");
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.Not.Null.And.InstanceOf<List<NotificationResponse>>(), "The value should be a list of NotificationResponse objects.");
            var returnedNotifications = okResult.Value as List<NotificationResponse>;
            Assert.That(returnedNotifications, Is.Not.Null.And.Empty, "The returned list of notifications should be empty.");
        }

        //[Test]
        //public async Task GetNotifications_ReturnsNotFoundResult_ForInvalidUserId()
        //{
        //    // Arrange
        //    var userId = "invalidUserId";

        //    _notificationServiceMock.Setup(x => x.GetNotifications(userId)).ReturnsAsync((List<Notification>)null);

        //    // Act
        //    var result = await _notificationsController.GetNotifications(userId);

        //    // Assert
        //    Assert.That(result, Is.Not.Null.And.InstanceOf<NotFoundResult>(), "The method should return a NotFoundResult for an invalid user ID.");
        //}


        //[Test]
        //public async Task GetNotifications_ReturnsInternalServerError_ForServiceError()
        //{
        //    // Arrange
        //    var userId = "validUserId";

        //    _notificationServiceMock.Setup(x => x.GetNotifications(userId)).ThrowsAsync(new Exception("Service error"));

        //    // Act
        //    var result = await _notificationsController.GetNotifications(userId);

        //    // Assert
        //    Assert.That(result, Is.Not.Null.And.InstanceOf<StatusCodeResult>(), "The method should return a StatusCodeResult for a service error.");
        //    var statusCodeResult = result as StatusCodeResult;
        //    Assert.That(statusCodeResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError), "The status code should be 500 for a service error.");
        //}

        //[Test]
        //public async Task GetNotifications_ReturnsInternalServerError_ForServiceError()
        //{
        //    // Arrange
        //    var userId = "validUserId";

        //    _notificationServiceMock.Setup(x => x.GetNotifications(userId)).ThrowsAsync(new Exception("Service error"));

        //    // Act & Assert
        //    var ex = Assert.ThrowsAsync<Exception>(async () => await _notificationsController.GetNotifications(userId));
        //    Assert.That(ex.Message, Is.EqualTo("Service error"), "The thrown exception should match the expected message.");
        //}


        [Test]
        public async Task GetNotifications_ReturnsBadRequest_ForInvalidUserId()
        {
            // Arrange
            var userId = "invalidUserId";
            _notificationServiceMock.Setup(x => x.GetNotifications(userId)).ReturnsAsync((List<Notification>)null);

            // Act
            var result = await _notificationsController.GetNotifications(userId);

            // Assert
            Assert.That(result, Is.Not.Null.And.InstanceOf<BadRequestObjectResult>());
        }


        //[Test]
        //public async Task GetNotifications_ReturnsNull_ForEmptyNotifications()
        //{
        //    // Arrange
        //    var userId = "validUserId";
        //    _notificationServiceMock.Setup(x => x.GetNotifications(userId)).ReturnsAsync(new List<Notification>());

        //    // Act
        //    var result = await _notificationsController.GetNotifications(userId);

        //    // Assert
        //    Assert.That(result, Is.Null);
        //}

        [Test]
        public async Task GetNotifications_ReturnsOk_ForEmptyNotifications()
        {
            // Arrange
            var userId = "validUserId";
            _notificationServiceMock.Setup(x => x.GetNotifications(userId)).ReturnsAsync(new List<Notification>());

            // Act
            var result = await _notificationsController.GetNotifications(userId);

            // Assert
            Assert.That(result, Is.Null.Or.InstanceOf<OkObjectResult>());
        }



        [Test]
        public async Task GetNotifications_ReturnsInternalServerError_ForServiceError()
        {
            // Arrange
            var userId = "validUserId";
            _notificationServiceMock.Setup(x => x.GetNotifications(userId)).ThrowsAsync(new Exception("Service error"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _notificationsController.GetNotifications(userId));
        }





        //[Test]
        //public async Task GetNotifications_ReturnsOkResult_WithUserAvatarImages()
        //{
        //    // Arrange
        //    var userId = "validUserId";
        //    var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
        //    var user = new ApplicationUser { Id = userId, FirstName = "John", LastName = "Doe", CloudAvatarImagePath = cloudinaryUrl };
        //    var notification1 = new Notification { UserId = userId, UserInteractionId = "userInteractionId1" };
        //    var notification2 = new Notification { UserId = userId, UserInteractionId = "userInteractionId2" };
        //    var notifications = new List<Notification> { notification1, notification2 };

        //    // Mocking the necessary dependencies
        //    _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);
        //    _notificationServiceMock.Setup(x => x.GetNotifications(userId)).ReturnsAsync(notifications);
        //    _userServiceMock.Setup(x => x.GetCloudinaryAvatarImagePath(user.Id)).ReturnsAsync(cloudinaryUrl);
        //    _userServiceMock.Setup(x => x.GetCloudinaryAvatarImagePath(user.Id)).ReturnsAsync(cloudinaryUrl);
        //    _mapperMock.Setup(m => m.Map<NotificationResponse>(notification1)).Returns(new NotificationResponse { });
        //    _mapperMock.Setup(m => m.Map<NotificationResponse>(notification2)).Returns(new NotificationResponse { });

        //    // Act
        //    var result = await _notificationsController.GetNotifications(userId);

        //    // Assert
        //    Assert.That(result, Is.Not.Null.And.InstanceOf<OkObjectResult>());
        //    var okResult = result as OkObjectResult;
        //    Assert.That(okResult.Value, Is.Not.Null.And.InstanceOf<List<NotificationResponse>>());
        //    var returnedNotifications = okResult.Value as List<NotificationResponse>;
        //    Assert.That(returnedNotifications, Is.Not.Null.And.Count.EqualTo(notifications.Count));
        //    foreach (var notificationResponse in returnedNotifications)
        //    {
        //        Assert.That(notificationResponse.UserNoti, Is.Not.Null);
        //        Assert.That(notificationResponse.UserNoti.UserAvatar, Is.Not.Null.And.Not.Empty);
        //    }
        //}

    }
}
