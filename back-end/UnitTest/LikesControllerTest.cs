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
    public class LikesControllerTest
    {
        private LikesController _likeController;
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

            _likeController = new LikesController(
                null,
                _mapperMock.Object,
                _likeServiceMock.Object,
                _articleServiceMock.Object,
                _dislikeServiceMock.Object,
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
        public async Task GetArticleLike_ArticleExists_Test()
        {
            var articleId = new Guid();
            var likes = new List<Like>(2);
            var interactResponse = new List<InteractResponse>();

            _articleServiceMock.Setup(a => a.ArticleExists( articleId )).Returns(true);
            _likeServiceMock.Setup(l => l.GetArticleLikes(articleId)).ReturnsAsync(likes);
            _mapperMock.Setup(m => m.Map<List<InteractResponse>>(likes)).Returns(interactResponse);

            var result = await _likeController.GetArticleLikes(articleId);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<InteractResponse>>());
        }

        [Test]
        public async Task GetArticleLike_ArticleNotExists_Test()
        {
            var articleId = new Guid();

            _articleServiceMock.Setup(a => a.ArticleExists(articleId)).Returns(false);

            var result = await _likeController.GetArticleLikes(articleId);

            //assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());

            
        }

        [Test]
        public async Task GetCommentLike_CommentExists_Test()
        {
            var commentId = new Guid();
            var likes = new List<Like>(2);
            var interactResponse = new List<InteractResponse>();

            _commentServiceMock.Setup(a => a.CommentExists(commentId)).Returns(true);
            _likeServiceMock.Setup(l => l.GetCommentLikes(commentId)).ReturnsAsync(likes);
            _mapperMock.Setup(m => m.Map<List<InteractResponse>>(likes)).Returns(interactResponse);

            var result = await _likeController.GetCommentLikes(commentId);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<InteractResponse>>());
        }

        [Test]
        public async Task GetCommentLike_CommentNotExists_Test()
        {
            var commentId = new Guid();

            _commentServiceMock.Setup(a => a.CommentExists(commentId)).Returns(false);

            var result = await _likeController.GetCommentLikes(commentId);

            //assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());


        }

        [Test]
        public async Task PostArticleLike_LikeNotExist_Test()
        {
            var articleId = new Guid();
            var userId = "string";
            var like = new Like();
            var likeDto = new ArticleInteractDTO 
            {
                UserId = userId,
                ArticleId = articleId,
            };
            var article = new Article 
            { 
                Title = "abcd",
                Description = "mot hai ba"
            };
            var user = new ApplicationUser
            {
                FirstName = "Vi Viet",
                LastName = "Quoc"
            };
            var message = "abcd";
            var notification = new Notification 
            { 
                Message = message,
                UserId = userId,
                UserInteractionId = "abcd"
            };
           
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var userNoti = new UserNoti
            {
                FirstName = "Vi Viet",
                LastName = "Quoc",
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

            _likeServiceMock.Setup(l => l.GetLikeByArticleAndUser(likeDto.ArticleId, likeDto.UserId)).ReturnsAsync((Like)null);
            _mapperMock.Setup(m => m.Map<Like>(likeDto)).Returns(like);
            _likeServiceMock.Setup(l => l.PostLike(like)).ReturnsAsync(true);
            _articleServiceMock.Setup(a => a.GetArticleByID(likeDto.ArticleId)).Returns(article);
            _userManagerMock.Setup(u => u.FindByIdAsync(likeDto.UserId)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.FindByIdAsync(article.StudentId)).ReturnsAsync(user);

            _notiServiceMock.Setup(n => n.GetNotiByUserAndArticle(article.StudentId, likeDto.ArticleId, message)).ReturnsAsync((Notification)null);
            _likeServiceMock.Setup(l => l.GetArticleLikesCount(likeDto.ArticleId)).ReturnsAsync(2);
            _notiServiceMock.Setup(n => n.PostNotification(notification)).ReturnsAsync(true);
            _mapperMock.Setup(m => m.Map<NotificationResponse>(notification)).Returns(notiResponse);
            _userServiceMock.Setup(u => u.GetCloudinaryAvatarImagePath(likeDto.UserId)).ReturnsAsync(cloudinaryUrl);
            _emailServiceMock.Setup(e => e.SendEmail(email)).ReturnsAsync(true);

            var result = await _likeController.PostArticleLike(likeDto);

            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        }

        [Test]
        public async Task PostArticleLike_LikeExist_Test()
        {
            var articleId = new Guid();
            var userId = "string";
            var like = new Like
            {
                UserId = userId,
                ArticleId = articleId,
            };
            var likeDto = new ArticleInteractDTO
            {
                UserId = userId,
                ArticleId = articleId,
            };

            _likeServiceMock.Setup(l => l.GetLikeByArticleAndUser(likeDto.ArticleId, likeDto.UserId)).ReturnsAsync(like);
            _likeServiceMock.Setup(l => l.DeleteLike(like)).ReturnsAsync(true);

            var result = await _likeController.PostArticleLike(likeDto);

            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var resultOk = (OkObjectResult)result.Result;

            Assert.That(resultOk.Value, Is.EqualTo("Existing like deleted successfully."));
        }

        [Test]
        public async Task PostArticleLike_ResultFalse_Test()
        {
            var articleId = new Guid();
            var userId = "string";
            var like = new Like
            {
                UserId = userId,
                ArticleId = articleId,
            };
            var likeDto = new ArticleInteractDTO
            {
                UserId = userId,
                ArticleId = articleId,
            };

            _likeServiceMock.Setup(l => l.GetLikeByArticleAndUser(likeDto.ArticleId, likeDto.UserId)).ReturnsAsync((Like)null);
            _mapperMock.Setup(m => m.Map<Like>(likeDto)).Returns(like);
            _likeServiceMock.Setup(l => l.PostLike(like)).ReturnsAsync(false);

            var result = await _likeController.PostArticleLike(likeDto);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }


        [Test]
        public async Task PostCommentLike_LikeNotExist_Test()
        {
            var commentId = new Guid();
            var userId = "string";
            var like = new Like();
            var likeDto = new CommentInteractDTO
            {
                UserId = userId,
                CommentId = commentId,
            };
            var comment = new Comment
            {
                Content = "abcd",
            };
            var user = new ApplicationUser
            {
                FirstName = "Vi Viet",
                LastName = "Quoc"
            };
            var message = "abcd";
            var notification = new Notification
            {
                Message = message,
                UserId = userId,
                UserInteractionId = "abcd"
            };

            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var userNoti = new UserNoti
            {
                FirstName = "Vi Viet",
                LastName = "Quoc",
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

            _likeServiceMock.Setup(l => l.GetLikeByCommentAndUser(likeDto.CommentId, likeDto.UserId)).ReturnsAsync((Like)null);
            _mapperMock.Setup(m => m.Map<Like>(likeDto)).Returns(like);
            _likeServiceMock.Setup(l => l.PostLike(like)).ReturnsAsync(true);
            _commentServiceMock.Setup(c => c.GetCommentById(likeDto.CommentId)).Returns(comment);
            _userManagerMock.Setup(u => u.FindByIdAsync(likeDto.UserId)).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.FindByIdAsync(comment.UserId)).ReturnsAsync(user);
            _notiServiceMock.Setup(n => n.GetNotiByUserAndComment(comment.UserId, likeDto.CommentId, message)).ReturnsAsync((Notification)null);
            _likeServiceMock.Setup(l => l.GetCommentLikesCount(likeDto.CommentId)).ReturnsAsync(2);
            _notiServiceMock.Setup(n => n.PostNotification(notification)).ReturnsAsync(true);
            _mapperMock.Setup(m => m.Map<NotificationResponse>(notification)).Returns(notiResponse);
            _userServiceMock.Setup(u => u.GetCloudinaryAvatarImagePath(likeDto.UserId)).ReturnsAsync(cloudinaryUrl);
            _emailServiceMock.Setup(e => e.SendEmail(email)).ReturnsAsync(true);

            var result = await _likeController.PostCommentLike(likeDto);

            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        }

        [Test]
        public async Task PostCommentLike_LikeExist_Test()
        {
            var commentId = new Guid();
            var userId = "string";
            var like = new Like
            {
                UserId = userId,
                CommentId = commentId,
            };
            var likeDto = new CommentInteractDTO
            {
                UserId = userId,
                CommentId = commentId,
            };
            var comment = new Comment
            {
                Content = "abcd",
            };

            _likeServiceMock.Setup(l => l.GetLikeByCommentAndUser(likeDto.CommentId, likeDto.UserId)).ReturnsAsync(like);
            _likeServiceMock.Setup(l => l.DeleteLike(like)).ReturnsAsync(true);

            var result = await _likeController.PostCommentLike(likeDto);

            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var resultOk = (OkObjectResult)result.Result;

            Assert.That(resultOk.Value, Is.EqualTo("Existing like deleted successfully."));
        }

        [Test]
        public async Task PostCommentLike_ResultFalse_Test()
        {
            var commentId = new Guid();
            var userId = "string";
            var like = new Like
            {
                UserId = userId,
                CommentId = commentId,
            };
            var likeDto = new CommentInteractDTO
            {
                UserId = userId,
                CommentId = commentId,
            };
            var comment = new Comment
            {
                Content = "abcd",
            };

            _likeServiceMock.Setup(l => l.GetLikeByCommentAndUser(likeDto.CommentId, likeDto.UserId)).ReturnsAsync((Like)null);
            _mapperMock.Setup(m => m.Map<Like>(likeDto)).Returns(like);
            _likeServiceMock.Setup(l => l.PostLike(like)).ReturnsAsync(false);

            var result = await _likeController.PostCommentLike(likeDto);

            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

    }

}

