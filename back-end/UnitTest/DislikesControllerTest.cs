using AutoMapper;
using Comp1640_Final.Controllers;
using Comp1640_Final.DTO.Request;
using Comp1640_Final.DTO.Response;
using Comp1640_Final.Models;
using Comp1640_Final.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class DislikesControllerTest
    {
        private DislikeController _dislikeController;
        private Mock<IMapper> _mapperMock;
        private Mock<ILikeService> _likeServiceMock;
        private Mock<IDislikeService> _dislikeServiceMock;
        private Mock<IArticleService> _articleServiceMock;
        private Mock<ICommentService> _commentServiceMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<INotificationService> _notiServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IEmailService> _emailServiceMock;

        [SetUp]
        public void SetUp()
        {
            _mapperMock = new Mock<IMapper>();
            _likeServiceMock = new Mock<ILikeService>();
            _dislikeServiceMock = new Mock<IDislikeService>();
            _articleServiceMock = new Mock<IArticleService>();
            _commentServiceMock = new Mock<ICommentService>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();// giả định lưu trữ và truy vấn thông tin người dùng
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _notiServiceMock = new Mock<INotificationService>();
            _userServiceMock = new Mock<IUserService>();
            _emailServiceMock = new Mock<IEmailService>();

            _dislikeController = new DislikeController(
                null,
                _mapperMock.Object,
                _dislikeServiceMock.Object,
                _articleServiceMock.Object,
                _likeServiceMock.Object,
                _commentServiceMock.Object,
                _userManagerMock.Object,
                null,
                _notiServiceMock.Object,
                _userServiceMock.Object,
                null,
                _emailServiceMock.Object
                );
        }

        [Test]
        public async Task GetArticleDislike_ArticleExists_Test()
        {
            var articleId = new Guid();
            var dislikes = new List<Dislike>(2);
            var interactResponse = new List<InteractResponse>();

            _articleServiceMock.Setup(a => a.ArticleExists(articleId)).Returns(true);
            _dislikeServiceMock.Setup(l => l.GetArticleDislikes(articleId)).ReturnsAsync(dislikes);
            _mapperMock.Setup(m => m.Map<List<InteractResponse>>(dislikes)).Returns(interactResponse);

            var result = await _dislikeController.GetArticleDislikes(articleId);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<InteractResponse>>());
        }

        [Test]
        public async Task GetArticleDislike_ArticleNotExists_Test()
        {
            var articleId = new Guid();

            _articleServiceMock.Setup(a => a.ArticleExists(articleId)).Returns(false);

            var result = await _dislikeController.GetArticleDislikes(articleId);

            //assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());


        }

        [Test]
        public async Task GetCommentDislike_CommentExists_Test()
        {
            var commentId = new Guid();
            var dislikes = new List<Dislike>(2);
            var interactResponse = new List<InteractResponse>();

            _commentServiceMock.Setup(a => a.CommentExists(commentId)).Returns(true);
            _dislikeServiceMock.Setup(l => l.GetCommentDislikes(commentId)).ReturnsAsync(dislikes);
            _mapperMock.Setup(m => m.Map<List<InteractResponse>>(dislikes)).Returns(interactResponse);

            var result = await _dislikeController.GetCommentDislikes(commentId);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<InteractResponse>>());
        }

        [Test]
        public async Task GetCommentDislike_CommentNotExists_Test()
        {
            var commentId = new Guid();

            _commentServiceMock.Setup(a => a.CommentExists(commentId)).Returns(false);

            var result = await _dislikeController.GetCommentDislikes(commentId);

            //assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());

        }

        [Test]
        public async Task PostArticleDislike_DislikeNotExist_Test()
        {
            var articleId = new Guid();
            var userId = "string";
            var dislike = new Dislike();
            var dislikeDto = new ArticleInteractDTO
            {
                UserId = userId,
                ArticleId = articleId,
            };
            var article = new Article
            {
                Title = "mot hai ba",
                Description = "sau bay tam"
            };
            var user = new ApplicationUser
            {
                FirstName = "Viet",
                LastName = "Vi",
                Email = "AAAAAAAAAA"
            };
            var message = "1212";
            var notification = new Notification
            {
                Message = message,
                UserId = userId,
                UserInteractionId = "afjhsdja"
            };

            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var userNoti = new UserNoti
            {
                FirstName = "Vi",
                LastName = "Viet",
                UserAvatar = cloudinaryUrl
            };
            var notiResponse = new NotificationResponse
            {
                UserNoti = userNoti
            };
            var email = new EmailDTO
            {
                Email = user.Email,
                Subject = "Some one disliked your post!",
                Body = message,
            };

            _dislikeServiceMock.Setup(l => l.GetDislikeByArticleAndUser(dislikeDto.ArticleId, dislikeDto.UserId)).ReturnsAsync((Dislike)null);
            _mapperMock.Setup(m => m.Map<Dislike>(dislikeDto)).Returns(dislike);
            _dislikeServiceMock.Setup(l => l.PostDislike(dislike)).ReturnsAsync(true);
            _articleServiceMock.Setup(a => a.GetArticleByID(dislikeDto.ArticleId)).Returns(article);
            _userManagerMock.Setup(u => u.FindByIdAsync(dislikeDto.UserId)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.FindByIdAsync(article.StudentId)).ReturnsAsync(user);
            _notiServiceMock.Setup(n => n.GetNotiByUserAndArticle(article.StudentId, dislikeDto.ArticleId, message)).ReturnsAsync((Notification)null);
            _dislikeServiceMock.Setup(l => l.GetArticleDislikesCount(dislikeDto.ArticleId)).ReturnsAsync(2);
            _notiServiceMock.Setup(n => n.PostNotification(notification)).ReturnsAsync(true);
            _mapperMock.Setup(m => m.Map<NotificationResponse>(notification)).Returns(notiResponse);
            _userServiceMock.Setup(u => u.GetCloudinaryAvatarImagePath(dislikeDto.UserId)).ReturnsAsync(cloudinaryUrl);
            _emailServiceMock.Setup(e => e.SendEmail(email)).ReturnsAsync(true);
            var result = await _dislikeController.PostArticleDislike(dislikeDto);

            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        }

        [Test]
        public async Task PostArticleDislike_DislikeExist_Test()
        {
            var articleId = new Guid();
            var userId = "string";
            var dislike = new Dislike
            {
                UserId = userId,
                ArticleId = articleId,
            };
            var dislikeDto = new ArticleInteractDTO
            {
                UserId = userId,
                ArticleId = articleId,
            };

            _dislikeServiceMock.Setup(d => d.GetDislikeByArticleAndUser(dislikeDto.ArticleId, dislikeDto.UserId)).ReturnsAsync(dislike);
            _dislikeServiceMock.Setup(d => d.DeleteDislike(dislike)).ReturnsAsync(true);

            var result = await _dislikeController.PostArticleDislike(dislikeDto);

            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var resultOk = (OkObjectResult)result.Result;

            Assert.That(resultOk.Value, Is.EqualTo("Dislike deleted successfully."));
        }

        [Test]
        public async Task PostArticleDislike_ResultFalse_Test()
        {
            var articleId = new Guid();
            var userId = "string";
            var dislike = new Dislike
            {
                UserId = userId,
                ArticleId = articleId,
            };
            var dislikeDto = new ArticleInteractDTO
            {
                UserId = userId,
                ArticleId = articleId,
            };

            _dislikeServiceMock.Setup(l => l.GetDislikeByArticleAndUser(dislikeDto.ArticleId, dislikeDto.UserId)).ReturnsAsync((Dislike)null);
            _mapperMock.Setup(m => m.Map<Dislike>(dislikeDto)).Returns(dislike);
            _dislikeServiceMock.Setup(l => l.PostDislike(dislike)).ReturnsAsync(false);

            var result = await _dislikeController.PostArticleDislike(dislikeDto);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task PostCommentDislike_DislikeNotExist_Test()
        {
            var commentId = new Guid();
            var userId = "string";
            var dislike = new Dislike();
            var dislikeDto = new CommentInteractDTO
            {
                UserId = userId,
                CommentId = commentId,
            };
            var comment = new Comment
            {
                Content = "adfsdf",
            };
            var user = new ApplicationUser
            {
                FirstName = "Viet",
                LastName = "Vi"
            };
            var message = "1212";
            var notification = new Notification
            {
                Message = message,
                UserId = userId,
                UserInteractionId = "afjhsdja"
            };

            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var userNoti = new UserNoti
            {
                FirstName = "Vi",
                LastName = "Viet",
                UserAvatar = cloudinaryUrl
            };
            var notiResponse = new NotificationResponse
            {
                UserNoti = userNoti
            };
            var email = new EmailDTO
            {
                Email = user.Email,
                Subject = "Some one disliked your post!",
                Body = message,
            };

            _dislikeServiceMock.Setup(l => l.GetDislikeByCommentAndUser(dislikeDto.CommentId, dislikeDto.UserId)).ReturnsAsync((Dislike)null);
            _mapperMock.Setup(m => m.Map<Dislike>(dislikeDto)).Returns(dislike);
            _dislikeServiceMock.Setup(l => l.PostDislike(dislike)).ReturnsAsync(true);
            _commentServiceMock.Setup(c => c.GetCommentById(dislikeDto.CommentId)).Returns(comment);
            _userManagerMock.Setup(u => u.FindByIdAsync(dislikeDto.UserId)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.FindByIdAsync(comment.UserId)).ReturnsAsync(user);
            _notiServiceMock.Setup(n => n.GetNotiByUserAndComment(comment.UserId, dislikeDto.CommentId, message)).ReturnsAsync((Notification)null);
            _likeServiceMock.Setup(l => l.GetCommentLikesCount(dislikeDto.CommentId)).ReturnsAsync(2);
            _notiServiceMock.Setup(n => n.PostNotification(notification)).ReturnsAsync(true);
            _mapperMock.Setup(m => m.Map<NotificationResponse>(notification)).Returns(notiResponse);
            _userServiceMock.Setup(u => u.GetCloudinaryAvatarImagePath(dislikeDto.UserId)).ReturnsAsync(cloudinaryUrl);

            var result = await _dislikeController.PostCommentDislike(dislikeDto);

            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        }

        [Test]
        public async Task PostCommentDislike_DislikeExist_Test()
        {
            var commentId = new Guid();
            var userId = "string";
            var dislike = new Dislike
            {
                UserId = userId,
                CommentId = commentId,
            };
            var dislikeDto = new CommentInteractDTO
            {
                UserId = userId,
                CommentId = commentId,
            };
            var comment = new Comment
            {
                Content = "abcd",
            };

            _dislikeServiceMock.Setup(l => l.GetDislikeByCommentAndUser(dislikeDto.CommentId, dislikeDto.UserId)).ReturnsAsync(dislike);
            _dislikeServiceMock.Setup(l => l.DeleteDislike(dislike)).ReturnsAsync(true);

            var result = await _dislikeController.PostCommentDislike(dislikeDto);

            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var resultOk = (OkObjectResult)result.Result;

            Assert.That(resultOk.Value, Is.EqualTo("Existing dislike deleted successfully."));
        }

        [Test]
        public async Task PostCommentDislike_ResultFalse_Test()
        {
            var commentId = new Guid();
            var userId = "string";
            var dislike = new Dislike
            {
                UserId = userId,
                CommentId = commentId,
            };
            var dislikeDto = new CommentInteractDTO
            {
                UserId = userId,
                CommentId = commentId,
            };
            var comment = new Comment
            {
                Content = "abcd",
            };

            _dislikeServiceMock.Setup(l => l.GetDislikeByCommentAndUser(dislikeDto.CommentId, dislikeDto.UserId)).ReturnsAsync((Dislike)null);
            _mapperMock.Setup(m => m.Map<Dislike>(dislikeDto)).Returns(dislike);
            _dislikeServiceMock.Setup(l => l.PostDislike(dislike)).ReturnsAsync(false);

            var result = await _dislikeController.PostCommentDislike(dislikeDto);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

    }
}
