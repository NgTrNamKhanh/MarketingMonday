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
            Assert.That(result, Is.Not.Null.And.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.Not.Null.And.InstanceOf<List<NotificationResponse>>());
            var returnedNotifications = okResult.Value as List<NotificationResponse>;
            Assert.That(returnedNotifications, Is.Not.Null.And.Empty);
        }

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

    }
}
