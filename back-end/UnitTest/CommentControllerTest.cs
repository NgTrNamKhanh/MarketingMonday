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
    public class CommentControllerTest
    {
        private CommentsController _commentController;
        private Mock<IMapper> _mapperMock;
        private Mock<ILikeService> _likeServiceMock;
        private Mock<IDislikeService> _dislikeServiceMock;
        private Mock<IArticleService> _articleServiceMock;
        private Mock<ICommentService> _commentServiceMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<INotificationService> _notiServiceMock;
        private Mock<IUserService> _userServiceMock;

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

            _commentController = new CommentsController(
                null,
                _mapperMock.Object,
                _userManagerMock.Object,
                _commentServiceMock.Object,
                null,
                _userServiceMock.Object,
                _likeServiceMock.Object,
                _dislikeServiceMock.Object,
                null,
                _articleServiceMock.Object,
                _notiServiceMock.Object
                );
        }

        [Test]
        public async Task GetComment_CommentExist_Test()
        {
            var userId = "random string";
            var comments = new List<Comment> 
            {
                new Comment
                {
                    Content = "ba hai mot",
                    UserId = userId,
                },
                new Comment
                {
                    Content = "asdasjdhas",
                    UserId = userId,
                }
            };

            var commentResponse = new CommentResponse
            {
                Content = "ba hai mot",
                hasReplies = true,
            };
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var user = new ApplicationUser
            {
                FirstName = "Vi Viet",
                LastName = "Quoc"
            };
            var curentRoles = new List<string> { "Guest", "Student" };

            _commentServiceMock.Setup(c => c.GetComments()).ReturnsAsync(comments);
            _mapperMock.Setup(m => m.Map<CommentResponse>(It.IsAny<Comment>())).Returns(commentResponse);
            _commentServiceMock.Setup(c => c.GetReplies(comments[0].Id)).ReturnsAsync(comments);
            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(curentRoles);
            _userServiceMock.Setup(u => u.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(cloudinaryUrl);
            _likeServiceMock.Setup(l => l.GetCommentLikesCount(comments[0].Id)).ReturnsAsync(3);
            _dislikeServiceMock.Setup(d => d.GetCommentDislikesCount(comments[0].Id)).ReturnsAsync(3);
            _likeServiceMock.Setup(l => l.IsCommentLiked(userId, comments[0].Id)).ReturnsAsync(true);
            _dislikeServiceMock.Setup(d => d.IsCommentDisLiked(userId, comments[0].Id)).ReturnsAsync(true);

            var result = await _commentController.GetComments(userId);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<CommentResponse>>());
        }

        [Test]
        public async Task GetComment_CommentNotExist_Test()
        {
            var userId = "random string";
            var comments = new List<Comment>
            {
                new Comment
                {
                    Content = "ba hai mot",
                    UserId = userId,
                },
                new Comment
                {
                    Content = "asdasjdhas",
                    UserId = userId,
                }
            };

            var commentResponse = new CommentResponse
            {
                Content = "ba hai mot",
                hasReplies = true,
            };
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";

            var curentRoles = new List<string> { "Guest", "Student" };

            _commentServiceMock.Setup(c => c.GetComments()).ReturnsAsync((List<Comment>)null);
            _mapperMock.Setup(m => m.Map<CommentResponse>(It.IsAny<Comment>())).Returns(commentResponse);
            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(curentRoles);

            var result = await _commentController.GetComments(userId);

            //assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetParentComments_CommentExist_Test()
        {
            var userId = "userId123";
            var articleId = Guid.NewGuid();
            var comments = new List<Comment>
            {
                new Comment
                {
                    Content = "ba hai mot",
                    UserId = userId,
                    ArticleId = articleId,
                },
                new Comment
                {
                    Content = "asdasjdhas",
                    UserId = userId,
                    ArticleId = articleId,
                }
            };

            var commentResponse = new CommentResponse
            {
                Content = "ba hai mot",
                hasReplies = true,
            };
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var user = new ApplicationUser
            {
                FirstName = "Vi Viet",
                LastName = "Quoc"
            };
            var curentRoles = new List<string> { "Guest", "Student" };

            _commentServiceMock.Setup(c => c.GetParentComments(articleId)).ReturnsAsync(comments);
            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(curentRoles);
            _mapperMock.Setup(m => m.Map<CommentResponse>(It.IsAny<Comment>())).Returns(commentResponse);
            _commentServiceMock.Setup(c => c.GetReplies(comments[0].Id)).ReturnsAsync(comments);
            _userServiceMock.Setup(u => u.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(cloudinaryUrl);
            _likeServiceMock.Setup(l => l.GetCommentLikesCount(comments[0].Id)).ReturnsAsync(3);
            _dislikeServiceMock.Setup(d => d.GetCommentDislikesCount(comments[0].Id)).ReturnsAsync(3);
            _likeServiceMock.Setup(l => l.IsCommentLiked(userId, comments[0].Id)).ReturnsAsync(true);
            _dislikeServiceMock.Setup(d => d.IsCommentDisLiked(userId, comments[0].Id)).ReturnsAsync(true);

            var result = await _commentController.GetParentComments(articleId, userId);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<CommentResponse>>());
        }

        [Test]
        public async Task GetParentComments_CommentNotExist_Test()
        {
            var userId = "random string";
            var articleId = Guid.NewGuid();
            var comments = new List<Comment>
            {
                new Comment
                {
                    Content = "ba hai mot",
                    UserId = userId,
                },
                new Comment
                {
                    Content = "asdasjdhas",
                    UserId = userId,
                }
            };

            var commentResponse = new CommentResponse
            {
                Content = "ba hai mot",
                hasReplies = true,
            };
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";

            var curentRoles = new List<string> { "Guest", "Student" };

            _commentServiceMock.Setup(c => c.GetParentComments(articleId)).ReturnsAsync((List<Comment>)null);
            _mapperMock.Setup(m => m.Map<CommentResponse>(It.IsAny<Comment>())).Returns(commentResponse);
            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(curentRoles);

            var result = await _commentController.GetComments(userId);

            //assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetReplies_RepliesExist_Test()
        {
            var userId = "userId123";
            var parentId = Guid.NewGuid();
            var comments = new List<Comment>
            {
                new Comment
                {
                    Content = "ba hai mot",
                    UserId = userId,
                    ParentCommentId = parentId,
                },
                new Comment
                {
                    Content = "asdasjdhas",
                    UserId = userId,
                    ParentCommentId = parentId,
                }
            };

            var commentResponse = new CommentResponse
            {
                Content = "ba hai mot",
                hasReplies = true,
            };
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            var user = new ApplicationUser
            {
                FirstName = "Vi Viet",
                LastName = "Quoc"
            };
            var curentRoles = new List<string> { "Guest", "Student" };

            _commentServiceMock.Setup(c => c.GetReplies(parentId)).ReturnsAsync(comments);
            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(curentRoles);
            _mapperMock.Setup(m => m.Map<CommentResponse>(It.IsAny<Comment>())).Returns(commentResponse);
            _commentServiceMock.Setup(c => c.GetReplies(comments[0].Id)).ReturnsAsync(comments);
            _userServiceMock.Setup(u => u.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(cloudinaryUrl);
            _likeServiceMock.Setup(l => l.GetCommentLikesCount(comments[0].Id)).ReturnsAsync(3);
            _dislikeServiceMock.Setup(d => d.GetCommentDislikesCount(comments[0].Id)).ReturnsAsync(3);
            _likeServiceMock.Setup(l => l.IsCommentLiked(userId, comments[0].Id)).ReturnsAsync(true);
            _dislikeServiceMock.Setup(d => d.IsCommentDisLiked(userId, comments[0].Id)).ReturnsAsync(true);

            var result = await _commentController.GetReplies(parentId, userId);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<CommentResponse>>());
        }


        [Test]
        public async Task GetReplies_RepliesNotExist_Test()
        {
            var userId = "random string";
            var parentId = Guid.NewGuid();
            var comments = new List<Comment>
            {
                new Comment
                {
                    Content = "ba hai mot",
                    UserId = userId,
                },
                new Comment
                {
                    Content = "asdasjdhas",
                    UserId = userId,
                }
            };

            var commentResponse = new CommentResponse
            {
                Content = "ba hai mot",
                hasReplies = true,
            };
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";

            var curentRoles = new List<string> { "Guest", "Student" };

            _commentServiceMock.Setup(c => c.GetReplies(parentId)).ReturnsAsync((List<Comment>)null);
            _mapperMock.Setup(m => m.Map<CommentResponse>(It.IsAny<Comment>())).Returns(commentResponse);
            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(curentRoles);

            var result = await _commentController.GetComments(userId);

            //assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task PostComment_ResultTrue_Test()
        {
            var articleId = Guid.NewGuid();
            var commentId = Guid.NewGuid();
            var userId = "ahsdjhaj";
            var comment = new Comment();
            var commentDto = new CommentDTO
            {
                Content = "mot hai ba",
                ArticleId = articleId,
                UserId = userId,
            };
            var commentResponse = new CommentResponse
            {
                Content = "ba hai mot",
                hasReplies = true,
            };
            var article = new Article
            {
                Title = "abcd",
                Description = "mot hai ba",
                StudentId = userId
                
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

            _mapperMock.Setup(m => m.Map<Comment>(commentDto)).Returns(comment);
            _commentServiceMock.Setup(c => c.PostComment(comment)).ReturnsAsync(true);
            _articleServiceMock.Setup(a => a.GetArticleByID(articleId)).Returns(article);
            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);
            _notiServiceMock.Setup(n => n.GetNotiByUserAndArticle(article.StudentId, commentDto.ArticleId, message)).ReturnsAsync((Notification)null);
            _commentServiceMock.Setup(c => c.GetCommentsCount(articleId)).ReturnsAsync(1);
            _notiServiceMock.Setup(n => n.PostNotification(notification)).ReturnsAsync(true);
            _commentServiceMock.Setup(c => c.GetCommentById(commentId)).Returns(comment);
            _mapperMock.Setup(m => m.Map<CommentResponse>(It.IsAny<Comment>())).Returns(commentResponse);
            _userServiceMock.Setup(u => u.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(cloudinaryUrl);

            var result = await _commentController.PostComment(commentDto);

            //assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var resultOk = (OkObjectResult)result.Result;

            Assert.That(resultOk.Value, Is.InstanceOf<CommentResponse>());
        }

        [Test]
        public async Task PostComment_ResultFalse_Test()
        {
            var articleId = Guid.NewGuid();
            var commentId = Guid.NewGuid();
            var userId = "ahsdjhaj";
            var comment = new Comment();
            var commentDto = new CommentDTO
            {
                Content = "mot hai ba",
                ArticleId = articleId,
                UserId = userId,
            };

            _mapperMock.Setup(m => m.Map<Comment>(commentDto)).Returns(comment);
            _commentServiceMock.Setup(c => c.PostComment(comment)).ReturnsAsync(false);

            var result = await _commentController.PostComment(commentDto);

            //assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());

        }

        [Test]
        public async Task PostReply_ParrentCommentExist_Test()
        {
            var articleId = Guid.NewGuid();
            var parentCommentId = Guid.NewGuid();
            var commentId = Guid.NewGuid();
            var userId = "ahsdjhaj";
            var comment = new Comment
            {
                Content = "asdasdas",
                UserId = userId,
                ParentCommentId = parentCommentId,
            };
            var commentDto = new CommentDTO
            {
                Content = "mot hai ba",
                ArticleId = articleId,
                UserId = userId,
                
            };
            var commentResponse = new CommentResponse
            {
                Content = "ba hai mot",
                hasReplies = true,
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

            _commentServiceMock.Setup(c => c.GetCommentById(parentCommentId)).Returns(comment);
            _mapperMock.Setup(m => m.Map<Comment>(commentDto)).Returns(comment);
            _commentServiceMock.Setup(c => c.PostComment(comment)).ReturnsAsync(true);
            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);
            _notiServiceMock.Setup(n => n.GetNotiByUserAndComment(comment.UserId, parentCommentId, message)).ReturnsAsync((Notification)null);
            _commentServiceMock.Setup(c => c.GetRepliesCount(parentCommentId)).ReturnsAsync(1);
            _notiServiceMock.Setup(n => n.PostNotification(notification)).ReturnsAsync(true);
            _mapperMock.Setup(m => m.Map<CommentResponse>(It.IsAny<Comment>())).Returns(commentResponse);
            _userServiceMock.Setup(u => u.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(cloudinaryUrl);

            var result = await _commentController.PostReply(parentCommentId ,commentDto);

            //assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var resultOk = (OkObjectResult)result.Result;

            Assert.That(resultOk.Value, Is.InstanceOf<CommentResponse>());
        }

        [Test]
        public async Task PostReply_ParrentCommentNotExist_Test()
        {
            var articleId = Guid.NewGuid();
            var parentCommentId = Guid.NewGuid();
            var commentId = Guid.NewGuid();
            var userId = "ahsdjhaj";
            var comment = new Comment
            {
                Content = "asdasdas",
                UserId = userId,
                ParentCommentId = parentCommentId,
            };
            var commentDto = new CommentDTO
            {
                Content = "mot hai ba",
                ArticleId = articleId,
                UserId = userId,

            };

            _commentServiceMock.Setup(c => c.GetCommentById(parentCommentId)).Returns((Comment)null);

            var result = await _commentController.PostReply(parentCommentId, commentDto);

            //asseert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());

        }

        [Test]
        public async Task PutComment_CommentExist_Test()
        {
            var parentCommentId = Guid.NewGuid();
            var commentId = Guid.NewGuid();
            var userId = "ahsdjhaj";
            var content = "asdasda";
            var comment = new Comment
            {
                Content = content,
                UserId = userId,
                ParentCommentId = parentCommentId,
            };
            var commentResponse = new CommentResponse
            {
                Content = "ba hai mot",
                hasReplies = true,
            };
            var user = new ApplicationUser
            {
                FirstName = "Vi Viet",
                LastName = "Quoc"
            };
            var cloudinaryUrl = "cloudinary://API_KEY:API_SECRET@CLOUD_NAME";
            _commentServiceMock.Setup(c => c.GetCommentById(commentId)).Returns(comment);
            _commentServiceMock.Setup(c => c.EditComment(comment)).ReturnsAsync(true);
            _userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<CommentResponse>(It.IsAny<Comment>())).Returns(commentResponse);
            _userServiceMock.Setup(u => u.GetCloudinaryAvatarImagePath(userId)).ReturnsAsync(cloudinaryUrl);

            var result = await _commentController.PutComment(commentId, content);

            //assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var resultOk = (OkObjectResult)result.Result;

            Assert.That(resultOk.Value, Is.InstanceOf<CommentResponse>());
        }

        [Test]
        public async Task PutComment_CommentNotExist_Test()
        {
            var parentCommentId = Guid.NewGuid();
            var commentId = Guid.NewGuid();
            var userId = "ahsdjhaj";
            var content = "asdasda";
            var comment = new Comment
            {
                Content = content,
                UserId = userId,
                ParentCommentId = parentCommentId,
            };

            _commentServiceMock.Setup(c => c.GetCommentById(commentId)).Returns((Comment)null);
           
            var result = await _commentController.PutComment(commentId, content);

            //assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());

         
        }

    }
}
